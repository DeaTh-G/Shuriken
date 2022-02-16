using System.IO;
using System.Collections.Generic;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWFontList : IBinarySerializable
    {
        public string Name { get; set; }
        public uint Field04 { get; set; }
        public uint Field08 { get; set; }
        public ushort FontMappingCount { get; set; }
        public ushort Field0E { get; set; }
        public uint FontMappingOffset { get; set; }
        public uint Field14 { get; set; }
        public uint Field18 { get; set; }
        public List<SWFontMapping> FontMappings { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.Read<uint>();
            Name = reader.ReadStringOffset(nameOffset, true);
            Field04 = reader.Read<uint>();
            Field08 = reader.Read<uint>();

            FontMappingCount = reader.Read<ushort>();
            Field0E = reader.Read<ushort>();
            FontMappingOffset = reader.Read<uint>();

            Field14 = reader.Read<uint>();
            Field18 = reader.Read<uint>();

            reader.PushOffsetOrigin();
            reader.Seek(FontMappingOffset, SeekOrigin.Begin);
            for (int i = 0; i < FontMappingCount; i++)
                FontMappings.Add(reader.ReadObject<SWFontMapping>());

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
