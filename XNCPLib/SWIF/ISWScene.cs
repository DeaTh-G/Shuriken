using Amicitia.IO.Binary;
using System.Collections.Generic;
using System.Numerics;

namespace XNCPLib.SWIF
{
    public interface ISWScene : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint Flags { get; set; }
        public uint LayerCount { get; set; }
        public long LayerOffset { get; set; }
        public long CameraCount { get; set; }
        public long CameraOffset { get; set; }
        public uint BackgroundColor { get; set; }
        public Vector2 FrameSize { get; set; }
        public long Field3C { get; set; }
        public List<ISWLayer> Layers { get; set; }
        public List<ISWCamera> Cameras { get; set; }
    }
}
