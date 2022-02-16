using System.IO;
using System.Collections.Generic;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

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
        public uint SubImageOffset { get; set; }
        public uint Field1C { get; set; }
        public List<SWSubImage> SubImages { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.Read<uint>();
            Name = reader.ReadStringOffset(nameOffset, true);

            ID = reader.Read<uint>();
            Width = reader.Read<ushort>();
            Height = reader.Read<ushort>();
            Flags = reader.Read<uint>();
            SubImageCount = reader.Read<uint>();
            SubImageOffset = reader.Read<uint>();
            Field1C = reader.Read<uint>();

            reader.PushOffsetOrigin();
            reader.Seek(SubImageOffset, SeekOrigin.Begin);
            for (int i = 0; i < SubImageCount; i++)
                SubImages.Add(reader.ReadObject<SWSubImage>());

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
