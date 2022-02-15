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
    public class SWTextureList
    {
        public string Name { get; set; }
        public uint TextureCount { get; set; }
        public uint TextureOffset { get; set; }
        public uint UserDataOffset { get; set; }
        public List<SWTexture> Textures { get; set; }

        public SWTextureList()
        {
            Textures = new List<SWTexture>();
        }

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.ReadUInt32();
            Name = reader.ReadAbsoluteStringOffset(nameOffset);

            TextureCount = reader.ReadUInt32();
            TextureOffset = reader.ReadUInt32();
            UserDataOffset = reader.ReadUInt32();

            reader.PushOffsetOrigin();
            reader.Seek(TextureOffset, SeekOrigin.Begin);
            for (int i = 0; i < TextureCount; i++)
            {
                SWTexture texture = new SWTexture();
                texture.Read(reader);

                Textures.Add(texture);
            }

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }
    }
}
