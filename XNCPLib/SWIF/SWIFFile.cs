using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWIFFile
    {
        public ChunkFile Content { get; set; }

        public SWIFFile()
        {
            Content = new ChunkFile();
        }

        public void Load(string filename)
        {
            BinaryObjectReader reader = new BinaryObjectReader(filename, Endianness.Little, Encoding.UTF8);
            reader.PushOffsetOrigin();

            // Signature
            reader.ReadUInt32();
            uint infoSize = reader.ReadUInt32();
            if (infoSize == 402653184)
                reader.Endianness = Endianness.Big;

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            Content.Read(reader);

            reader.PopOffsetOrigin();

            reader.Dispose();
        }
    }
}
