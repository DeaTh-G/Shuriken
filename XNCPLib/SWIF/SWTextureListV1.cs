using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWTextureListV1 : IBinarySerializable
    {
        public string Name { get; set; }
        public uint TextureCount { get; set; }
        public uint TextureOffset { get; set; }
        public uint Field0C { get; set; }
        public List<SWTextureV1> Textures { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            TextureCount = reader.Read<uint>();
            TextureOffset = reader.Read<uint>();
            reader.ReadAtOffset(TextureOffset, () =>
            {
                for (int i = 0; i < TextureCount; i++)
                    Textures.Add(reader.ReadObject<SWTextureV1>());
            });

            Field0C = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
