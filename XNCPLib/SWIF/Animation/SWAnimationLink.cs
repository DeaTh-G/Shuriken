using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimationLink
    {
        public ushort CastID { get; set; }
        public ushort Field02 { get; set; } // Track Count?
        public uint Field04 { get; set; } // Track Offset?

        public SWAnimationLink() { }

        public void Read(BinaryObjectReader reader)
        {
            CastID = reader.ReadUInt16();

            Field02 = reader.ReadUInt16();
            Field04 = reader.ReadUInt32();
        }
    }
}
