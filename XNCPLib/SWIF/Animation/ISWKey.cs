using System.Runtime.InteropServices;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public interface ISWKey : IBinarySerializable<uint>
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct ValueUnion
        {
            [FieldOffset(0)] public float Float;
            [FieldOffset(0)] public int Integer;
            [FieldOffset(0)] public bool Boolean;
            [FieldOffset(0)] public char Character;
            [FieldOffset(0)] public uint Color;
            [FieldOffset(0)] public uint UnsignedInteger;
            [FieldOffset(0)] public double Double;

            public void Set(float f) { Float = f; }
            public void Set(int i) { Integer = i; }
            public void Set(bool b) { Boolean = b; }
            public void Set(char c) { Character = c; }
            public void Set(uint ui) { UnsignedInteger = ui; }
            public void Set(double d) { Double = d; }
        }

        public int Keyframe { get; set; }
        public ValueUnion Value { get; set; }
    }
}
