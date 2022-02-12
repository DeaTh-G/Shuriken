using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWCast
    {
        public StringOffset Name { get; set; }
        public int ID { get; set; }
        public uint Flags { get; set; }
        public uint CastInfoOffset { get; set; }
        public short ChildIndex { get; set; }
        public short NextIndex { get; set; }
        public uint Field14 { get; set; }
        public SWCastInfo CastInfo { get; set; }

        public SWCast()
        {
            Name = new StringOffset();
            CastInfo = new SWCastInfo();
        }

        public void Read(BinaryObjectReader reader)
        {
            Name.Read(reader);
            ID = reader.ReadInt32();
            Flags = reader.ReadUInt32();

            CastInfoOffset = reader.ReadUInt32();
            ChildIndex = reader.ReadInt16();
            NextIndex = reader.ReadInt16();
            Field14 = reader.ReadUInt32();

            if (CastInfoOffset != 0)
            {
                reader.PushOffsetOrigin();

                reader.Seek(CastInfoOffset, SeekOrigin.Begin);
                CastInfo.Read(reader);

                reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
                reader.PopOffsetOrigin();
            }
        }
    }
}
