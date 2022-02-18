using System.IO;
using Amicitia.IO.Binary;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWProjectChunk : IBinarySerializable<uint>
    {
        public ChunkHeader Header { get; set; } = new();
        public uint ProjectNodeOffset { get; set; }
        public uint Field0C { get; set; }
        public SWProjectNode ProjectNode { get; set; } = new();

        public void Read(BinaryObjectReader reader, uint version)
        {
            var origin = reader.Position;
            Header = reader.ReadObject<ChunkHeader>();

            ProjectNodeOffset = reader.Read<uint>();
            Field0C = reader.Read<uint>();

            ProjectNode = reader.ReadObjectAtOffset<SWProjectNode, uint>(origin + ProjectNodeOffset, version);

            reader.Seek(origin + Header.Size + 8, SeekOrigin.Begin);
        }

        public void Write(BinaryObjectWriter writer, uint version) { }
    }
}
