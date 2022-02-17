using System.Collections.Generic;
using Amicitia.IO.Binary;
using XNCPLib.SWIF.Cast;
using XNCPLib.SWIF.Animation;

namespace XNCPLib.SWIF
{
    public class SWLayerV1 : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint Flags { get; set; }
        public uint CastCellCount { get; set; }
        public uint CastNodeOffset { get; set; }
        public uint CellOffset { get; set; }
        public uint AnimationCount { get; set; }
        public uint AnimationOffset { get; set; }
        public uint Field20 { get; set; }
        public uint Field24 { get; set; }
        public List<SWCastNodeV1> CastNodes { get; set; } = new();
        public List<SWCellV1> Cells { get; set; } = new();
        public List<SWAnimationV1> Animations { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            ID = reader.Read<uint>();
            Flags = reader.Read<uint>();

            CastCellCount = reader.Read<uint>();
            CastNodeOffset = reader.Read<uint>();
            CellOffset = reader.Read<uint>();

            AnimationCount = reader.Read<uint>();
            AnimationOffset = reader.Read<uint>();

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
                    Cells.Add(reader.ReadObject<SWCellV1>());
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
