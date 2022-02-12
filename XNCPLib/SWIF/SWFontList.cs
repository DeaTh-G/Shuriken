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
        public StringOffset Name { get; set; }
        public uint Field04 { get; set; }
        public uint Field08 { get; set; }
        public ushort FontCount { get; set; }
        public ushort Field0E { get; set; }
        public uint FontOffset { get; set; }
        public uint Field14 { get; set; }
        public uint Field18 { get; set; }
        public List<SWFontMapping> FontsMappings { get; set; }

        public SWFontList()
        {
            Name = new StringOffset();
            FontsMappings = new List<SWFontMapping>();
        }

        public void Read(BinaryObjectReader reader)
        {
            Name.Read(reader);
            Field04 = reader.ReadUInt32();
            Field08 = reader.ReadUInt32();

            FontCount = reader.ReadUInt16();
            Field0E = reader.ReadUInt16();
            FontOffset = reader.ReadUInt32();

            Field14 = reader.ReadUInt32();
            Field18 = reader.ReadUInt32();

            reader.PushOffsetOrigin();
            reader.Seek(FontOffset, SeekOrigin.Begin);
            for (int i = 0; i < FontCount; i++)
            {
                SWFontMapping fontMap = new SWFontMapping();
                fontMap.Read(reader);

                FontsMappings.Add(fontMap);
            }

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }
    }
}
