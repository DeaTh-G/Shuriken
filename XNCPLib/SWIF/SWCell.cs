using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Amicitia.IO.Binary;
using System.Runtime.InteropServices;

namespace XNCPLib.SWIF
{
    [StructLayout(LayoutKind.Explicit)]
    public struct SWCellInfoField
    {
        [FieldOffset(0)] public uint integer;
        [FieldOffset(0)] public float single;

        public SWCellInfoField(uint value) : this() { integer = value; }
    }

    public class SWCellInfo
    {
        public Vector3 Position { get; set; }
        public SWCellInfoField Field0C { get; set; }
        public SWCellInfoField Field10 { get; set; }
        public ushort Rotation { get; set; }
        public ushort Field16 { get; set; }
        public Vector3 Scale { get; set; }

        public SWCellInfo()
        {
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            Scale = new Vector3(1.0f, 1.0f, 1.0f);

            Field0C = new SWCellInfoField();
            Field10 = new SWCellInfoField();
        }

        public void Read(BinaryObjectReader reader)
        {
            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Field0C = new SWCellInfoField(reader.ReadUInt32());
            Field10 = new SWCellInfoField(reader.ReadUInt32());
            Rotation = reader.ReadUInt16();
            Field16 = reader.ReadUInt16();

            if (Field16 == 0)
                Scale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }

    public class SWCell
    {
        public uint Color { get; set; }
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
            CellInfo = new SWCellInfo();
        }

        public void Read(BinaryObjectReader reader)
        {
            Color = reader.ReadUInt32();
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
