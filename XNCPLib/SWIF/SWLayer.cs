using System.IO;
using System.Collections.Generic;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;
using XNCPLib.SWIF.Cast;
using XNCPLib.SWIF.Animation;

namespace XNCPLib.SWIF
{
    public class SWLayer : IBinarySerializable
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
        public List<SWCastNode> Casts { get; set; } = new();
        public List<SWCell> Cells { get; set; } = new();
        public List<SWAnimation> Animations { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.Read<uint>();
            Name = reader.ReadStringOffset(nameOffset, true);
            ID = reader.Read<uint>();
            Flags = reader.Read<uint>();

            CastCellCount = reader.Read<uint>();
            CastNodeOffset = reader.Read<uint>();
            CellOffset = reader.Read<uint>();

            AnimationCount = reader.Read<uint>();
            AnimationOffset = reader.Read<uint>();

            Field20 = reader.Read<uint>();
            Field24 = reader.Read<uint>();

            reader.PushOffsetOrigin();
            reader.Seek(CastNodeOffset, SeekOrigin.Begin);
            for (int i = 0; i < CastCellCount; i++)
                Casts.Add(reader.ReadObject<SWCastNode>());

            reader.Seek(CellOffset, SeekOrigin.Begin);
            for (int i = 0; i < CastCellCount; i++)
                Cells.Add(reader.ReadObject<SWCell>());

            reader.Seek(AnimationOffset, SeekOrigin.Begin);
            for (int i = 0; i < AnimationCount; i++)
                Animations.Add(reader.ReadObject<SWAnimation>());

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
