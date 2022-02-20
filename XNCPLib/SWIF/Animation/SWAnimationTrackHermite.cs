using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimationTrackHermite : ISWAnimationTrack
    {
        public int Keyframe { get; set; }

        public ISWAnimationTrack.ValueUnion Value { get; set; }
        public float Field08 { get; set; }
        public float Field0C { get; set; }

        public void Read(BinaryObjectReader reader, uint Flags)
        {
            Keyframe = reader.Read<int>();

            var union = new ISWAnimationTrack.ValueUnion();
            switch (Flags & 0xF0)
            {
                case 0x10:
                    reader.Read(out union.Float);
                    Field08 = reader.Read<float>();
                    Field0C = reader.Read<float>();
                    break;
                case 0x40:
                    reader.Read(out union.Integer);
                    break;
                case 0x60:
                    reader.Read(out union.UnsignedInteger);
                    break;
                case 0x70:
                    reader.Read(out union.Double);
                    break;
            }

            Value = union;
        }

        public void Write(BinaryObjectWriter writer, uint Flags) { }
    }
}
