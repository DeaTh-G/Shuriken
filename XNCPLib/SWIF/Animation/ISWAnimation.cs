using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public interface ISWAnimation : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint AnimationLinkCount { get; set; }
        public long Field10 { get; set; }
        public long AnimationLinkOffset { get; set; }
        public uint Field20 { get; set; }
        public byte Field24 { get; set; }
        public byte Field25 { get; set; }
        public byte Field26 { get; set; }
        public byte Field27 { get; set; }
        public List<SWAnimationLink> AnimationLinks { get; set; }
    }
}
