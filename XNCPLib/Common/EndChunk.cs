using Amicitia.IO.Binary;

namespace XNCPLib.Common
{
    public class EndChunk : IBinarySerializable
    {
        public ChunkHeader Header { get; set; } = new();
        public uint[] Padding { get; set; } = new uint[] { 0, 0 };

        public void Read(BinaryObjectReader reader)
        {
            Header = reader.ReadObject<ChunkHeader>();
            
            for (int i = 0; i < 2; i++)
                Padding[i] = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
