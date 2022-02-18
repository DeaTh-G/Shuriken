using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public interface ISWCamera : IBinarySerializable
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
    }
}
