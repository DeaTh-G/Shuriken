using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast
{
    public class SWCell : IBinarySerializable<uint>
    {
        public uint Color { get; set; }
        public byte Field04 { get; set; }
        public byte Field05 { get; set; }
        public byte Field06 { get; set; }
        public byte Field07 { get; set; }
        public byte Field08 { get; set; }
        public byte Field09 { get; set; }
        public byte Field0A { get; set; }
        public byte Field0B { get; set; }
        public ISWCellInfo CellInfo { get; set; }

        public void Read(BinaryObjectReader reader, uint version)
        {
            for (int i = 0; i < 4; i++)
                Color |= (uint)(reader.Read<byte>() << 8 * i);

            Field04 = reader.Read<byte>();
            Field05 = reader.Read<byte>();
            Field06 = reader.Read<byte>();
            Field07 = reader.Read<byte>();
            Field08 = reader.Read<byte>();
            Field09 = reader.Read<byte>();
            Field0A = reader.Read<byte>();
            Field0B = reader.Read<byte>();

            if (version == 1)
                CellInfo = reader.ReadObject<SWCellInfoV1>();
            else if (version == 2)
                CellInfo = reader.ReadObject<SWCellInfoV2>();
        }

        public void Write(BinaryObjectWriter writer, uint version) { }
    }
}
