using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public interface ISWTextureList : IBinarySerializable
    {
        public string Name { get; set; }
        public uint TextureCount { get; set; }
        public long TextureOffset { get; set; }
        public long Field0C { get; set; }
        public List<SWTexture> Textures { get; set; }
    }
}
