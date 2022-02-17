using Amicitia.IO.Binary;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWIFFileV1 : IBinarySerializable
    {
        public SWInfoChunk Info { get; set; } = new();
        public SWTextureListChunkV1 TextureList { get; set; } = new();
        public SWProjectChunkV1 Project { get; set; } = new();
        public OffsetChunk Offset { get; set; } = new();
        public EndChunk End { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Info = reader.ReadObject<SWInfoChunk>();
            TextureList = reader.ReadObject<SWTextureListChunkV1>();
            Project = reader.ReadObject<SWProjectChunkV1>();
            Offset = reader.ReadObject<OffsetChunk>();
            End = reader.ReadObject<EndChunk>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
