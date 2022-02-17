using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast
{
    public class SWCellInfo : IBinarySerializable
    {
        public Vector3 Position { get; set; }
        public uint Field0C { get; set; }
        public uint Field10 { get; set; }
        public uint Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            Position = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
            Field0C = reader.Read<uint>();
            Field10 = reader.Read<uint>();
            Rotation = reader.Read<uint>();

            Scale = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
