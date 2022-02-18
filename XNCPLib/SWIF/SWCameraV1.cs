using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWCameraV1 : ISWCamera
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public uint Flags { get; set; }
        public float RangeIn { get; set; }
        public float RangeOut { get; set; }
        public uint Field48 { get; set; }
        public uint Field4C { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            ID = reader.Read<uint>();

            Position = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
            LookAt = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());

            Flags = reader.Read<uint>();
            RangeIn = reader.Read<float>();
            RangeOut = reader.Read<float>();

            Field48 = reader.Read<uint>();
            Field4C = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
