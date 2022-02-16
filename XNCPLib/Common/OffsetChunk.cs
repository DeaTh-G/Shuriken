using System.IO;
using System.Collections.Generic;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

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
            reader.PushOffsetOrigin();

            OffsetLocationCount = reader.Read<uint>();
            Field0C = reader.Read<uint>();

            for (int i = 0; i < OffsetLocationCount; ++i)
                OffsetLocations.Add(reader.Read<uint>());

            reader.Seek(reader.GetOffsetOrigin() + Header.Size, SeekOrigin.Begin);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
