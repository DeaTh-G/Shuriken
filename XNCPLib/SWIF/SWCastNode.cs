using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;
using System.Runtime.InteropServices;

namespace XNCPLib.SWIF
{
    public class SWCastNode
    {
        public enum EFlags
        {
            eFlags_Default = 0,
            eFlags_ImageCast = 1,
            eFlags_SliceCast = 2,
            eFlags_ReferenceCast = 3
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SWCast
        {
            [FieldOffset(0)] public SWImageCast ImageCast;

            public SWCast(SWImageCast cast) : this()
            {
                ImageCast = cast;
            }
        }

        public string Name { get; set; }
        public int ID { get; set; }
        public uint Flags { get; set; }
        public uint CastOffset { get; set; }
        public short ChildIndex { get; set; }
        public short NextIndex { get; set; }
        public uint Field14 { get; set; }
        public SWCast Cast { get; set; }

        public SWCastNode()
        {
            Cast = new SWCast(new SWImageCast());
        }

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.ReadUInt32();
            Name = reader.ReadAbsoluteStringOffset(nameOffset);

            ID = reader.ReadInt32();
            Flags = reader.ReadUInt32();

            CastOffset = reader.ReadUInt32();
            ChildIndex = reader.ReadInt16();
            NextIndex = reader.ReadInt16();
            Field14 = reader.ReadUInt32();

            if (CastOffset != 0)
            {
                reader.PushOffsetOrigin();

                reader.Seek(CastOffset, SeekOrigin.Begin);
                Cast.ImageCast.Read(reader);

                reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
                reader.PopOffsetOrigin();
            }
        }
    }
}
