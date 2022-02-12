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
    public class SWTextureListChunk
    {
        public ChunkHeader Header { get; set; }
        public uint ListOffset { get; set; }
        public uint ListCount { get; set; }
        public List<SWTextureList> TextureLists { get; set; }

        public SWTextureListChunk()
        {
            Header = new ChunkHeader();
            TextureLists = new List<SWTextureList>();
        }

        public void Read(BinaryObjectReader reader)
        {
            reader.PushOffsetOrigin();
            Header.Read(reader);

            ListOffset = reader.ReadUInt32();
            ListCount = reader.ReadUInt32();

            reader.Seek(reader.GetOffsetOrigin() + ListOffset, SeekOrigin.Begin);
            for (int i = 0; i < ListCount; i++)
            {
                SWTextureList textureList = new SWTextureList();
                textureList.Read(reader);

                TextureLists.Add(textureList);
            }

            reader.PopOffsetOrigin();
            reader.Seek(Header.EndPosition, SeekOrigin.Begin);
        }
    }
}
