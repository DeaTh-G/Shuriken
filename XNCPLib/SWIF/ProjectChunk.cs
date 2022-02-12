using System;
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
    public class ProjectChunk
    {
        public ChunkHeader Header { get; set; }
        public uint ProjectNodeOffset { get; set; }
        public uint Padding { get; set; }
        public SWProject Project { get; set; }

        public ProjectChunk()
        {
            Header = new ChunkHeader();
            Project = new SWProject();
        }

        public void Read(BinaryObjectReader reader)
        {
            reader.PushOffsetOrigin();
            Header.Read(reader);

            ProjectNodeOffset = reader.ReadUInt32();
            Padding = reader.ReadUInt32();

            reader.Seek(reader.GetOffsetOrigin() + ProjectNodeOffset, SeekOrigin.Begin);
            Project.Read(reader);

            reader.PopOffsetOrigin();
            reader.Seek(Header.EndPosition, SeekOrigin.Begin);
        }
    }
}
