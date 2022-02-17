using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast
{
    public class SWCastNodeV1 : IBinarySerializable
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public uint Flags { get; set; }
        public uint CastOffset { get; set; }
        public short ChildIndex { get; set; }
        public short NextIndex { get; set; }
        public uint Field14 { get; set; }
        public SWImageCastV1 ImageCast { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

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
