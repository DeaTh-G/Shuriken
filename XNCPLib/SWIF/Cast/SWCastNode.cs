using System.IO;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF.Cast
{
    public class SWCastNode : IBinarySerializable
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public uint Flags { get; set; }
        public uint CastOffset { get; set; }
        public short ChildIndex { get; set; }
        public short NextIndex { get; set; }
        public uint Field14 { get; set; }
        public SWImageCast ImageCast { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.Read<uint>();
            Name = reader.ReadStringOffset(nameOffset, true);

            ID = reader.Read<int>();
            Flags = reader.Read<uint>();

            CastOffset = reader.Read<uint>();
            ChildIndex = reader.Read<short>();
            NextIndex = reader.Read<short>();
            Field14 = reader.Read<uint>();

            if (CastOffset != 0)
            {
                reader.PushOffsetOrigin();
                reader.Seek(CastOffset, SeekOrigin.Begin);

                switch (Flags & 0xF)
                {
                    case 1:
                        ImageCast = reader.ReadObject<SWImageCast>();
                        break;
                    default:
                        break;
                }

                reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
                reader.PopOffsetOrigin();
            }
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
