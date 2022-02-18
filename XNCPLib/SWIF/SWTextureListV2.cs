using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWTextureListV2 : ISWTextureList
    {
        public string Name { get; set; }
        public uint Field08 { get; set; }
        public uint TextureCount { get; set; }
        public long TextureOffset { get; set; }
        public long Field0C { get; set; }
        public List<SWTexture> Textures { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            Field08 = reader.Read<uint>();
            TextureCount = reader.Read<uint>();
            TextureOffset = reader.ReadOffsetValue();
            reader.ReadAtOffset(TextureOffset, () =>
            {
                for (int i = 0; i < TextureCount; i++)
                    Textures.Add(reader.ReadObject<SWTexture>());
            });

            Field0C = reader.ReadOffsetValue();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
