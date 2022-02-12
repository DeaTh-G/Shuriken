using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class InfoChunk
    {
        public ChunkHeader Header { get; set; }
        public uint ChunkCount { get; set; }
        public uint NextChunkOffset { get; set; }
        public uint ChunkListSize { get; set; }
        public uint OffsetChunkOffset { get; set; }
        public uint Flags { get; set; }
        public uint Padding { get; set; }
        
        public InfoChunk()
        {
            Header = new ChunkHeader();
        }

        public void Read(BinaryObjectReader reader)
        {
            Header.Read(reader);

            ChunkCount = reader.ReadUInt32();
            NextChunkOffset = reader.ReadUInt32();
            ChunkListSize = reader.ReadUInt32();
            OffsetChunkOffset = reader.ReadUInt32();
            Flags = reader.ReadUInt32();
            Padding = reader.ReadUInt32();
        }
    }
}
