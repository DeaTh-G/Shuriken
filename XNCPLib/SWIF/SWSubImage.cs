using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWSubImage
    {
        public Vector2 TopLeft { get; set; }
        public Vector2 BottomRight { get; set; }

        public SWSubImage()
        {
            TopLeft = new Vector2(0.0f, 0.0f);
            BottomRight = new Vector2(1.0f, 1.0f);
        }
        public void Read(BinaryObjectReader reader)
        {
            TopLeft = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            BottomRight = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
