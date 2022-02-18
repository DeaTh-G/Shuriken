using System.Collections.Generic;
using Amicitia.IO.Binary;
using XNCPLib.SWIF.Cast;
using XNCPLib.SWIF.Animation;

namespace XNCPLib.SWIF
{
    public class SWLayerV1 : ISWLayer
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
        public List<ISWCastNode> CastNodes { get; set; } = new();
        public List<SWCell> Cells { get; set; } = new();
        public List<SWAnimationV1> Animations { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            ID = reader.Read<uint>();
            Flags = reader.Read<uint>();

            CastCellCount = reader.ReadOffsetValue();
            CastNodeOffset = reader.ReadOffsetValue();
            CellOffset = reader.ReadOffsetValue();

            AnimationCount = reader.ReadOffsetValue();
            AnimationOffset = reader.ReadOffsetValue();

            Field20 = reader.Read<uint>();
            Field24 = reader.Read<uint>();

            reader.ReadAtOffset(CastNodeOffset, () =>
            {
                for (int i = 0; i < CastCellCount; i++)
                    CastNodes.Add(reader.ReadObject<SWCastNodeV1>());
            });

            reader.ReadAtOffset(CellOffset, () =>
            {
                for (int i = 0; i < CastCellCount; i++)
                    Cells.Add(reader.ReadObject<SWCell, uint>(1));
            });

            reader.ReadAtOffset(AnimationOffset, () =>
            {
                for (int i = 0; i < AnimationCount; i++)
                    Animations.Add(reader.ReadObject<SWAnimationV1>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
