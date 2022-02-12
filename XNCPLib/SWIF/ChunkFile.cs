using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class ChunkFile
    {
        public InfoChunk Info { get; set; }
        public SWTextureListChunk TextureList { get; set; }
        public ProjectChunk SurfWaveProject { get; set; }
        public OffsetChunk Offset { get; set; }
        public EndChunk End { get; set; }

        public ChunkFile()
        {
            Info = new InfoChunk();
            TextureList = new SWTextureListChunk();
            SurfWaveProject = new ProjectChunk();
            Offset = new OffsetChunk();
            End = new EndChunk();
        }

        public void Read(BinaryObjectReader reader)
        {
            Info.Read(reader);

            reader.Seek(Info.NextChunkOffset, SeekOrigin.Begin);
            if (Info.ChunkCount >= 2)
            {
                TextureList.Read(reader);
                SurfWaveProject.Read(reader);
            }
            if (Info.ChunkCount >= 1 && Info.ChunkCount < 2)
            {
                SurfWaveProject.Read(reader);
            }

            Offset.Read(reader);
            End.Read(reader);
        }
    }
}
