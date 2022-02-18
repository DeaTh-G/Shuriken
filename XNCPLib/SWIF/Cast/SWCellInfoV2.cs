using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast
{
    public class SWCellInfoV2 : ISWCellInfo
    {
        public uint Field00 { get; set; }
        public Vector3 Position { get; set; }
        public uint Field10 { get; set; }
        public uint Field14 { get; set; }
        public uint Field18 { get; set; }
        public uint Field1C { get; set; }
        public uint Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public uint Field30 { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            Field00 = reader.Read<uint>();
            Position = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
            Field10 = reader.Read<uint>();
            Field14 = reader.Read<uint>();
            Field18 = reader.Read<uint>();
            Field1C = reader.Read<uint>();
            Rotation = reader.Read<uint>();

            Scale = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
            
            Field30 = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
