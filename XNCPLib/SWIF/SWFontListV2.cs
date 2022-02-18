using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWFontListV2 : ISWFontList
    {
        public string Name { get; set; }
        public uint Field08 { get; set; }
        public uint Field0C { get; set; }
        public ushort Field10 { get; set; }
        public ushort FontMappingCount { get; set; }
        public ushort Field14 { get; set; }
        public ushort Field16 { get; set; }
        public long FontMappingOffset { get; set; }
        public uint Field20 { get; set; }
        public uint Field24 { get; set; }
        public uint Field28 { get; set; }
        public uint Field2C { get; set; }
        public List<SWFontMapping> FontMappings { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            Field08 = reader.Read<uint>();
            Field0C = reader.Read<uint>();
            Field10 = reader.Read<ushort>();

            FontMappingCount = reader.Read<ushort>();
            Field14 = reader.Read<ushort>();
            Field16 = reader.Read<ushort>();
            FontMappingOffset = reader.ReadOffsetValue();

            Field20 = reader.Read<uint>();
            Field24 = reader.Read<uint>();
            Field28 = reader.Read<uint>();
            Field2C = reader.Read<uint>();

            reader.ReadAtOffset(FontMappingOffset, () =>
            {
                for (int i = 0; i < FontMappingCount; i++)
                    FontMappings.Add(reader.ReadObject<SWFontMapping>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
