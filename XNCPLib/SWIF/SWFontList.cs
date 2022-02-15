using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWFontList
    {
        public string Name { get; set; }
        public uint Field04 { get; set; }
        public uint Field08 { get; set; }
        public ushort FontMappingCount { get; set; }
        public ushort Field0E { get; set; }
        public uint FontMappingOffset { get; set; }
        public uint Field14 { get; set; }
        public uint Field18 { get; set; }
        public List<SWFontMapping> FontMappings { get; set; }

        public SWFontList()
        {
            FontMappings = new List<SWFontMapping>();
        }

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.ReadUInt32();
            Name = reader.ReadAbsoluteStringOffset(nameOffset);
            Field04 = reader.ReadUInt32();
            Field08 = reader.ReadUInt32();

            FontMappingCount = reader.ReadUInt16();
            Field0E = reader.ReadUInt16();
            FontMappingOffset = reader.ReadUInt32();

            Field14 = reader.ReadUInt32();
            Field18 = reader.ReadUInt32();

            reader.PushOffsetOrigin();
            reader.Seek(FontMappingOffset, SeekOrigin.Begin);
            for (int i = 0; i < FontMappingCount; i++)
            {
                SWFontMapping fontMap = new SWFontMapping();
                fontMap.Read(reader);

                FontMappings.Add(fontMap);
            }

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }
    }
}
