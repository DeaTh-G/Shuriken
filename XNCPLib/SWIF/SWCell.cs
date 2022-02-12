using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWCellInfo
    {
        public Vector3 Position { get; set; }
        public uint Field0C { get; set; }
        public uint Field10 { get; set; }
        public uint Field14 { get; set; }
        public float Field18 { get; set; }
        public float Field1C { get; set; }
        public float Field20 { get; set; }

        public SWCellInfo()
        {
            Position = new Vector3(0.0f, 1.0f, 0.0f);
        }

        public void Read(BinaryObjectReader reader)
        {
            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Field0C = reader.ReadUInt32();
            Field10 = reader.ReadUInt32();
            Field14 = reader.ReadUInt32();
            Field18 = reader.ReadSingle();
            Field1C = reader.ReadSingle();
            Field20 = reader.ReadSingle();
        }
    }

    public class SWCell
    {
        public SWColor Color { get; set; }
        public byte Field04 { get; set; }
        public byte Field05 { get; set; }
        public byte Field06 { get; set; }
        public byte Field07 { get; set; }
        public byte Field08 { get; set; }
        public byte Field09 { get; set; }
        public byte Field0A { get; set; }
        public byte Field0B { get; set; }
        public SWCellInfo CellInfo { get; set; }

        public SWCell()
        {
            Color = new SWColor();
            CellInfo = new SWCellInfo();
        }

        public void Read(BinaryObjectReader reader)
        {
            Color.Read(reader);
            Field04 = reader.ReadByte();
            Field05 = reader.ReadByte();
            Field06 = reader.ReadByte();
            Field07 = reader.ReadByte();
            Field08 = reader.ReadByte();
            Field09 = reader.ReadByte();
            Field0A = reader.ReadByte();
            Field0B = reader.ReadByte();
            CellInfo.Read(reader);
        }
    }
}
