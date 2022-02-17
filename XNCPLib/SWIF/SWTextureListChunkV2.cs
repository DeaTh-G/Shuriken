﻿using System.IO;
using System.Collections.Generic;
using Amicitia.IO.Binary;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWTextureListChunkV2 : IBinarySerializable
    {
        public ChunkHeader Header { get; set; } = new();
        public uint ListOffset { get; set; }
        public uint ListCount { get; set; }
        public List<SWTextureListV2> TextureLists { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            var origin = reader.Position;
            Header = reader.ReadObject<ChunkHeader>();

            ListOffset = reader.Read<uint>();
            ListCount = reader.Read<uint>();
            reader.ReadAtOffset(origin + ListOffset, () =>
            {
                for (int i = 0; i < ListCount; i++)
                    TextureLists.Add(reader.ReadObject<SWTextureListV2>());
            });

            reader.Seek(origin + Header.Size + 8, SeekOrigin.Begin);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
