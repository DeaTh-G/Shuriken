using System.Numerics;
using System.Runtime.InteropServices;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast
{
    [StructLayout(LayoutKind.Explicit)]
    public struct SWCellInfoField
    {
        [FieldOffset(0)] public uint integer;
        [FieldOffset(0)] public float single;

        public SWCellInfoField(uint value) : this() { integer = value; }
    }

    public class SWCellInfo : IBinarySerializable
    {
        public Vector3 Position { get; set; }
        public SWCellInfoField Field0C { get; set; } = new();
        public SWCellInfoField Field10 { get; set; } = new();
        public ushort Rotation { get; set; }
        public ushort Field16 { get; set; }
        public Vector3 Scale { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            Position = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
            Field0C = new SWCellInfoField(reader.Read<uint>());
            Field10 = new SWCellInfoField(reader.Read<uint>());
            Rotation = reader.Read<ushort>();
            Field16 = reader.Read<ushort>();

            if (Field16 == 0)
                Scale = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
