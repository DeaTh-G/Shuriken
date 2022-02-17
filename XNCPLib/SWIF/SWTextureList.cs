using System.IO;
using System.Collections.Generic;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWTextureList : IBinarySerializable
    {
        public string Name { get; set; }
        public uint TextureCount { get; set; }
        public uint TextureOffset { get; set; }
        public uint Field0C { get; set; }
        public List<SWTexture> Textures { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Name = reader.ReadStringOffset(reader.Read<uint>(), true);

            TextureCount = reader.Read<uint>();
            TextureOffset = reader.Read<uint>();
            reader.ReadAtOffset(TextureOffset, () =>
            {
                for (int i = 0; i < TextureCount; i++)
                    Textures.Add(reader.ReadObject<SWTexture>());
            });

            Field0C = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
