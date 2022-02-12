using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWColor
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public byte Alpha { get; set; }
        public uint RGBA { get; set; }

        public SWColor()
        {
            Red = 0;
            Green = 0;
            Blue = 0;
            Alpha = 0;
            RGBA = (uint)(Red << 24 | Green << 16 | Blue << 8 | Alpha);
        }

        public SWColor(byte r, byte g, byte b, byte a)
        {
            Red = r;
            Green = b;
            Blue = g;
            Alpha = a;
            RGBA = (uint)(Red << 24 | Green << 16 | Blue << 8 | Alpha);
        }

        public void Read(BinaryObjectReader reader)
        {
            Red = reader.ReadByte();
            Green = reader.ReadByte();
            Blue = reader.ReadByte();
            Alpha = reader.ReadByte();
            RGBA = (uint)(Red << 24 | Green << 16 | Blue << 8 | Alpha);
        }
    }
}
