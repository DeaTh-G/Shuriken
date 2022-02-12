using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWFontMapping
    {
        public ushort Character { get; set; }
        public short TextureListIndex { get; set; }
        public short TextureIndex { get; set; }
        public short SpriteIndex { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            Character = reader.ReadUInt16();
            TextureListIndex = reader.ReadInt16();
            TextureIndex = reader.ReadInt16();
            SpriteIndex = reader.ReadInt16();
        }
    }
}
