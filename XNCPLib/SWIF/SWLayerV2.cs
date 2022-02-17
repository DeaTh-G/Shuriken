using System.Collections.Generic;
using Amicitia.IO.Binary;
using XNCPLib.SWIF.Cast;
using XNCPLib.SWIF.Animation;

namespace XNCPLib.SWIF
{
    public class SWLayerV2 : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint Flags { get; set; }
        public ulong CastCellCount { get; set; }
        public ulong CastNodeOffset { get; set; }
        public ulong CellOffset { get; set; }
        public ulong AnimationCount { get; set; }
        public ulong AnimationOffset { get; set; }
        public uint Field20 { get; set; }
        public uint Field24 { get; set; }
        public ulong Field28 { get; set; }
        public List<SWCastNodeV2> CastNodes { get; set; } = new();
        public List<SWCellV2> Cells { get; set; } = new();
        public List<SWAnimationV2> Animations { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            ID = reader.Read<uint>();
            Flags = reader.Read<uint>();

            CastCellCount = reader.Read<ulong>();
            CastNodeOffset = reader.Read<ulong>();
            CellOffset = reader.Read<ulong>();

            AnimationCount = reader.Read<ulong>();
            AnimationOffset = reader.Read<ulong>();

            Field20 = reader.Read<uint>();
            Field24 = reader.Read<uint>();
            Field28 = reader.Read<ulong>();

            reader.ReadAtOffset((long)CastNodeOffset, () =>
            {
                for (ulong i = 0; i < CastCellCount; i++)
                    CastNodes.Add(reader.ReadObject<SWCastNodeV2>());
            });

            reader.ReadAtOffset((long)CellOffset, () =>
            {
                for (ulong i = 0; i < CastCellCount; i++)
                    Cells.Add(reader.ReadObject<SWCellV2>());
            });

            reader.ReadAtOffset((long)AnimationOffset, () =>
            {
                for (ulong i = 0; i < AnimationCount; i++)
                    Animations.Add(reader.ReadObject<SWAnimationV2>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
