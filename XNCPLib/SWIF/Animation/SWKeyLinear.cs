using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public class SWKeyLinear : ISWKey
    {
        public int Keyframe { get; set; }

        public ISWKey.ValueUnion Value { get; set; } = new();

        public void Read(BinaryObjectReader reader, uint Flags)
        {
            Keyframe = reader.Read<int>();

            var union = new ISWKey.ValueUnion();
            switch (Flags & 0xF0)
            {
                case 0x10:
                    reader.Read(out union.Float);
                    break;
                case 0x20:
                    reader.Read(out union.Integer);
                    break;
                case 0x30:
                    reader.Read(out union.Boolean);
                    break;
                case 0x40:
                    reader.Read(out union.Integer);
                    break;
                case 0x50:
                    reader.Read(out union.Color);
                    break;
            }

            Value = union;
        }

        public void Write(BinaryObjectWriter writer, uint Flags) { }
    }
}
