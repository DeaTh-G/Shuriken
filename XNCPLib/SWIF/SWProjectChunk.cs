using System.IO;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWProjectChunk : IBinarySerializable
    {
        public ChunkHeader Header { get; set; } = new();
        public uint ProjectNodeOffset { get; set; }
        public uint Field0C { get; set; }
        public SWProject Project { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Header = reader.ReadObject<ChunkHeader>();
            reader.PushOffsetOrigin();

            ProjectNodeOffset = reader.Read<uint>();
            Field0C = reader.Read<uint>();

            reader.Seek(reader.GetOffsetOrigin() - 8 + ProjectNodeOffset, SeekOrigin.Begin);
            Project = reader.ReadObject<SWProject>();

            reader.Seek(reader.GetOffsetOrigin() + Header.Size, SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
