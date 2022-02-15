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
    public class SWTexture
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public uint Flags { get; set; }
        public uint SubImageCount { get; set; }
        public uint SubImageOffset { get; set; }
        public uint Field1C { get; set; }
        public List<SWSubImage> SubImages { get; set; }

        public SWTexture()
        {
            SubImages = new List<SWSubImage>();
        }

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.ReadUInt32();
            Name = reader.ReadAbsoluteStringOffset(nameOffset);

            ID = reader.ReadUInt32();
            Width = reader.ReadUInt16();
            Height = reader.ReadUInt16();
            Flags = reader.ReadUInt32();
            SubImageCount = reader.ReadUInt32();
            SubImageOffset = reader.ReadUInt32();
            Field1C = reader.ReadUInt32();
            reader.PushOffsetOrigin();

            reader.Seek(SubImageOffset, SeekOrigin.Begin);
            for (int i = 0; i < SubImageCount; i++)
            {
                SWSubImage subImage = new SWSubImage();
                subImage.Read(reader);

                SubImages.Add(subImage);
            }

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }
    }
}
