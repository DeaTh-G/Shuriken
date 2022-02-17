using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWCameraV2 : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID;
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public uint Field24 { get; set; }
        public uint Field28 { get; set; }
        public uint Field2C { get; set; }
        public uint Field30 { get; set; }
        public uint Field34 { get; set; }
        public uint Field38 { get; set; }
        public uint Flags { get; set; }
        public float RangeIn { get; set; }
        public float RangeOut { get; set; }
        public uint Field48 { get; set; }
        public uint Field4C { get; set; }
        public uint Field50 { get; set; }
        public uint Field54 { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            ID = reader.Read<uint>();

            Position = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
            LookAt = new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());

            Field24 = reader.Read<uint>();
            Field28 = reader.Read<uint>();
            Field2C = reader.Read<uint>();
            Field30 = reader.Read<uint>();
            Field34 = reader.Read<uint>();
            Field38 = reader.Read<uint>();

            Flags = reader.Read<uint>();
            RangeIn = reader.Read<float>();
            RangeOut = reader.Read<float>();

            Field48 = reader.Read<uint>();
            Field4C = reader.Read<uint>();
            Field50 = reader.Read<uint>();
            Field54 = reader.Read<uint>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
