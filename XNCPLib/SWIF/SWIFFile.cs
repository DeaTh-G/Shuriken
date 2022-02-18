using Amicitia.IO.Binary;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWIFFile : IBinarySerializable<uint>
    {
        public SWInfoChunk Info { get; set; } = new();
        public SWTextureListChunk TextureList { get; set; } = new();
        public SWProjectChunk Project { get; set; } = new();
        public OffsetChunk Offset { get; set; } = new();
        public EndChunk End { get; set; } = new();

        public void Read(BinaryObjectReader reader, uint version)
        {
            Info = reader.ReadObject<SWInfoChunk>();
            TextureList = reader.ReadObject<SWTextureListChunk, uint>(version);
            Project = reader.ReadObject<SWProjectChunk, uint>(version);
            Offset = reader.ReadObject<OffsetChunk>();
            End = reader.ReadObject<EndChunk>();
        }

        public void Write(BinaryObjectWriter writer, uint version) { }
    }
}
