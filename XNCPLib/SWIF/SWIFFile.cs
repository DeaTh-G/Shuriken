using Amicitia.IO.Binary;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWIFFile : IBinarySerializable
    {
        public SWInfoChunk Info { get; set; } = new();
        public SWTextureListChunk TextureList { get; set; } = new();
        public SWProjectChunk Project { get; set; } = new();
        public OffsetChunk Offset { get; set; } = new();
        public EndChunk End { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Info = reader.ReadObject<SWInfoChunk>();
            TextureList = reader.ReadObject<SWTextureListChunk>();
            Project = reader.ReadObject<SWProjectChunk>();
            Offset = reader.ReadObject<OffsetChunk>();
            End = reader.ReadObject<EndChunk>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
