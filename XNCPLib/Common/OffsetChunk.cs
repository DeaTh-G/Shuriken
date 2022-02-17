using System.Collections.Generic;
using System.IO;
using Amicitia.IO.Binary;

namespace XNCPLib.Common
{
    public class OffsetChunk : IBinarySerializable
    {
        public ChunkHeader Header { get; set; } = new();
        public uint OffsetLocationCount { get; set; }
        public uint Field0C { get; set; }
        public List<uint> OffsetLocations { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Header = reader.ReadObject<ChunkHeader>();

            OffsetLocationCount = reader.Read<uint>();
            Field0C = reader.Read<uint>();

            for (int i = 0; i < OffsetLocationCount; ++i)
                OffsetLocations.Add(reader.Read<uint>());

            reader.Seek(reader.Position + (0x10 - reader.Position % 16), SeekOrigin.Begin);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
