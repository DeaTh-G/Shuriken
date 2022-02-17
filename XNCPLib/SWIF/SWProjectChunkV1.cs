﻿using System.IO;
using Amicitia.IO.Binary;
using XNCPLib.Common;

namespace XNCPLib.SWIF
{
    public class SWProjectChunkV1 : IBinarySerializable
    {
        public ChunkHeader Header { get; set; } = new();
        public uint ProjectNodeOffset { get; set; }
        public uint Field0C { get; set; }
        public SWProjectNodeV1 ProjectNode { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            var origin = reader.Position;
            Header = reader.ReadObject<ChunkHeader>();

            ProjectNodeOffset = reader.Read<uint>();
            Field0C = reader.Read<uint>();

            ProjectNode = reader.ReadObjectAtOffset<SWProjectNodeV1>(origin + ProjectNodeOffset);

            reader.Seek(origin + Header.Size + 8, SeekOrigin.Begin);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
