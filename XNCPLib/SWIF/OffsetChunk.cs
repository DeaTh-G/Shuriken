﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class OffsetChunk
    {
        public ChunkHeader Header { get; set; }
        public uint OffsetLocationCount { get; set; }
        public uint Field0C { get; set; }
        public List<uint> OffsetLocations { get; set; }
        
        public OffsetChunk()
        {
            Header = new ChunkHeader();
            OffsetLocations = new List<uint>();
        }

        public void Read(BinaryObjectReader reader)
        {
            reader.PushOffsetOrigin();
            Header.Read(reader);

            OffsetLocationCount = reader.ReadUInt32();
            Field0C = reader.ReadUInt32();

            Header.EndPosition = (uint)reader.GetOffsetOrigin() + OffsetLocationCount * 4;
            for (int i = 0; i < OffsetLocationCount; i++)
            {
                OffsetLocations.Add(reader.ReadUInt32());
            }

            reader.Seek(Header.EndPosition, SeekOrigin.Begin);
        }
    }
}
