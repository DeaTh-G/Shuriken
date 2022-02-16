using System.IO;
using System.Collections.Generic;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWTextureListChunk : IBinarySerializable
    {
        public ChunkHeader Header { get; set; } = new();
        public uint ListOffset { get; set; }
        public uint ListCount { get; set; }
        public List<SWTextureList> TextureLists { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Header = reader.ReadObject<ChunkHeader>();
            reader.PushOffsetOrigin();

            ListOffset = reader.Read<uint>();
            ListCount = reader.Read<uint>();
            reader.ReadAtOffset(ListOffset - 8, () =>
            {
                for (int i = 0; i < ListCount; i++)
                    TextureLists.Add(reader.ReadObject<SWTextureList>());
            });

            reader.Seek(reader.GetOffsetOrigin() + Header.Size, SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
