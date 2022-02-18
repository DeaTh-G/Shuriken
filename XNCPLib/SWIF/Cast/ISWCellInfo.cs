using Amicitia.IO.Binary;
using System.Numerics;

namespace XNCPLib.SWIF.Cast
{
    public interface ISWCellInfo : IBinarySerializable
    {
        public Vector3 Position { get; set; }
        public uint Field10 { get; set; }
        public uint Field14 { get; set; }
        public uint Rotation { get; set; }
        public Vector3 Scale { get; set; }
    }
}
