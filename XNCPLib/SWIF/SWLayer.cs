using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;
using XNCPLib.SWIF.Animation;

namespace XNCPLib.SWIF
{
    public class SWLayer
    {
        public enum EFlags
        {
            eFlags_Enabled = 1
        }

        public StringOffset Name { get; set; }
        public uint ID { get; set; }
        public EFlags Flags { get; set; }
        public uint CastCellCount { get; set; }
        public uint CastOffset { get; set; }
        public uint CellOffset { get; set; }
        public uint AnimationCount { get; set; }
        public uint AnimationOffset { get; set; }
        public uint Field20 { get; set; }
        public uint Field24 { get; set; }
        public List<SWCast> Casts { get; set; }
        public List<SWCell> Cells { get; set; }
        public List<SWAnimation> Animations { get; set; }

        public SWLayer()
        {
            Name = new StringOffset();
            Casts = new List<SWCast>();
            Cells = new List<SWCell>();
            Animations = new List<SWAnimation>();
        }

        public void Read(BinaryObjectReader reader)
        {
            Name.Read(reader);
            ID = reader.ReadUInt32();
            Flags = (EFlags)reader.ReadUInt32();

            CastCellCount = reader.ReadUInt32();
            CastOffset = reader.ReadUInt32();
            CellOffset = reader.ReadUInt32();

            AnimationCount = reader.ReadUInt32();
            AnimationOffset = reader.ReadUInt32();

            Field20 = reader.ReadUInt32();
            Field24 = reader.ReadUInt32();
            reader.PushOffsetOrigin();

            reader.Seek(CastOffset, SeekOrigin.Begin);
            for (int i = 0; i < CastCellCount; i++)
            {
                SWCast cast = new SWCast();
                cast.Read(reader);

                Casts.Add(cast);
            }

            reader.Seek(CellOffset, SeekOrigin.Begin);
            for (int i = 0; i < CastCellCount; i++)
            {
                SWCell cell = new SWCell();
                cell.Read(reader);

                Cells.Add(cell);
            }

            reader.Seek(AnimationOffset, SeekOrigin.Begin);
            for (int i = 0; i < AnimationCount; i++)
            {
                SWAnimation animation = new SWAnimation();
                animation.Read(reader);

                Animations.Add(animation);
            }

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }
    }
}
