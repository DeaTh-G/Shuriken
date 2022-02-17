using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast
{
    public class SWCell : IBinarySerializable
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
        public SWCellInfo CellInfo { get; set; } = new();

        public void Read(BinaryObjectReader reader)
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
            CellInfo = reader.ReadObject<SWCellInfo>();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
