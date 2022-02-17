using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWCamera : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID;
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public uint Flags { get; set; }
        public float RangeIn { get; set; }
        public float RangeOut { get; set; }
        public uint Field2C { get; set; }
        public uint Field30 { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            ID = reader.Read<uint>();

            Position = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
            LookAt = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());

            Flags = reader.Read<uint>();
            RangeIn = reader.Read<float>();
            RangeOut = reader.Read<float>();

            Field2C = reader.Read<uint>();
            Field30 = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
