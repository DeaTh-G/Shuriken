using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast
{
    public class SWCastNodeV1 : ISWCastNode
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public uint Flags { get; set; }
        public long CastOffset { get; set; }
        public short ChildIndex { get; set; }
        public short NextIndex { get; set; }
        public uint Field1C { get; set; }
        public ISWImageCast ImageCast { get; set; } = new SWImageCastV1();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            ID = reader.Read<int>();
            Flags = reader.Read<uint>();

            CastOffset = reader.ReadOffsetValue();
            ChildIndex = reader.Read<short>();
            NextIndex = reader.Read<short>();
            Field1C = reader.Read<uint>();

            if (CastOffset != 0)
            {
                reader.ReadAtOffset(CastOffset, () =>
                {
                    switch (Flags & 0xF)
                    {
                        case 1:
                            ImageCast = reader.ReadObject<SWImageCastV1>();
                            break;
                        default:
                            break;
                    }
                });
            }
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
