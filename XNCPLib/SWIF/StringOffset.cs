using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class StringOffset
    {
        public uint Offset { get; set; }
        public string Value { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            Offset = reader.ReadUInt32();
            long pos = reader.Position;

            if (Offset != 0)
            {
                reader.Seek(Offset, SeekOrigin.Begin);
                Value = reader.ReadString(StringBinaryFormat.NullTerminated);
                reader.Seek(pos, SeekOrigin.Begin);
            }
        }
    }
}
