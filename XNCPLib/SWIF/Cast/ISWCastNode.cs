using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast
{
    public interface ISWCastNode : IBinarySerializable
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public uint Flags { get; set; }
        public long CastOffset { get; set; }
        public short ChildIndex { get; set; }
        public short NextIndex { get; set; }
        public uint Field1C { get; set; }
        public ISWImageCast ImageCast { get; set; }
    }
}
