using Amicitia.IO.Binary;
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
            Name = reader.ReadStringOffset(reader.Read<uint>(), true);

            ID = reader.Read<int>();
            Flags = reader.Read<uint>();

            CastOffset = reader.Read<uint>();
            ChildIndex = reader.Read<short>();
            NextIndex = reader.Read<short>();
            Field14 = reader.Read<uint>();

            if (CastOffset != 0)
            {
                reader.ReadAtOffset(CastOffset, () =>
                {
                    switch (Flags & 0xF)
                    {
                        case 1:
                            ImageCast = reader.ReadObject<SWImageCast>();
                            break;
                        default:
                            break;
                    }
                }, true);
            }
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
