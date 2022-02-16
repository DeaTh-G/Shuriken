using Amicitia.IO.Binary;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWInfoChunk : IBinarySerializable
    {
        public ChunkHeader Header { get; set; } = new();
        public uint ChunkCount { get; set; }
        public uint NextChunkOffset { get; set; } = 32;
        public uint ChunkListSize { get; set; }
        public uint OffsetChunkOffset { get; set; }
        public uint Field18 { get; set; } = 0x01330481;
        public uint Field1C { get; set; } // Always 0

        public void Read(BinaryObjectReader reader)
        {
            Header = reader.ReadObject<ChunkHeader>();
            if (Header.Size == 402653184)
                reader.Endianness = Endianness.Big;

            ChunkCount = reader.Read<uint>();
            NextChunkOffset = reader.Read<uint>();
            ChunkListSize = reader.Read<uint>();
            OffsetChunkOffset = reader.Read<uint>();
            Field18 = reader.Read<uint>();
            Field1C = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
