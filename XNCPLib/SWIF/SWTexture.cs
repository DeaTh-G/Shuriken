using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWTexture : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public uint Flags { get; set; }
        public uint SubImageCount { get; set; }
        public long SubImageOffset { get; set; }
        public long Field1C { get; set; }
        public List<SWSubImage> SubImages { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            ID = reader.Read<uint>();
            Width = reader.Read<ushort>();
            Height = reader.Read<ushort>();
            Flags = reader.Read<uint>();
            SubImageCount = reader.Read<uint>();
            SubImageOffset = reader.ReadOffsetValue();
            Field1C = reader.ReadOffsetValue();

            reader.ReadAtOffset(SubImageOffset, () =>
            {
                for (int i = 0; i < SubImageCount; i++)
                    SubImages.Add(reader.ReadObject<SWSubImage>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
