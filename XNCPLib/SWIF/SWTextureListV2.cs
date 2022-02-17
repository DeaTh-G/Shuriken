using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWTextureListV2 : IBinarySerializable
    {
        public string Name { get; set; }
        public uint Field08 { get; set; }
        public uint TextureCount { get; set; }
        public ulong TextureOffset { get; set; }
        public ulong Field0C { get; set; }
        public List<SWTextureV2> Textures { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            Field08 = reader.Read<uint>();
            TextureCount = reader.Read<uint>();
            TextureOffset = reader.Read<ulong>();
            reader.ReadAtOffset((long)TextureOffset, () =>
            {
                for (int i = 0; i < TextureCount; i++)
                    Textures.Add(reader.ReadObject<SWTextureV2>());
            });

            Field0C = reader.Read<ulong>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
