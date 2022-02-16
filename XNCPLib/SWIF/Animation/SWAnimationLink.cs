using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimationLink : IBinarySerializable
    {
        public ushort CastID { get; set; }
        public ushort Field02 { get; set; } // Track Count?
        public uint Field04 { get; set; } // Track Offset?

        public void Read(BinaryObjectReader reader)
        {
            CastID = reader.Read<ushort>();

            Field02 = reader.Read<ushort>();
            Field04 = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
