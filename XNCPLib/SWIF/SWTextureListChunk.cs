using System.IO;
using System.Collections.Generic;
using Amicitia.IO.Binary;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWTextureListChunk : IBinarySerializable<uint>
    {
        public ChunkHeader Header { get; set; } = new();
        public uint ListOffset { get; set; }
        public uint ListCount { get; set; }
        public List<ISWTextureList> TextureLists { get; set; } = new();

        public void Read(BinaryObjectReader reader, uint version)
        {
            var origin = reader.Position;
            Header = reader.ReadObject<ChunkHeader>();
            
            ListOffset = reader.Read<uint>();
            ListCount = reader.Read<uint>();
            reader.ReadAtOffset(origin + ListOffset, () =>
            {
                for (int i = 0; i < ListCount; i++)
                {
                    if (version == 1)
                        TextureLists.Add(reader.ReadObject<SWTextureListV1>());
                    else if (version == 2)
                        TextureLists.Add(reader.ReadObject<SWTextureListV2>());
                }
            });

            reader.Seek(origin + Header.Size + 8, SeekOrigin.Begin);
        }

        public void Write(BinaryObjectWriter writer, uint version) { }
    }
}
