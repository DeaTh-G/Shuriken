using System.Collections.Generic;
using Amicitia.IO.Binary;
using XNCPLib.SWIF.Cast;

namespace XNCPLib.SWIF
{
    public interface ISWLayer : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint Flags { get; set; }
        public long CastCellCount { get; set; }
        public long CastNodeOffset { get; set; }
        public long CellOffset { get; set; }
        public long AnimationCount { get; set; }
        public long AnimationOffset { get; set; }
        public uint Field20 { get; set; }
        public uint Field24 { get; set; }
        public List<ISWCastNode> CastNodes { get; set; }
        public List<SWCell> Cells { get; set; }
    }
}
