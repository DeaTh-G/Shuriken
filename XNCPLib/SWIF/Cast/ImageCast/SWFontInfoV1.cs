using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast.ImageCast
{
    public class SWFontInfoV1 : IBinarySerializable
    {
        public uint Field00 { get; set; }
        public uint FontListIndex { get; set; }
        public uint CharacterListOffset { get; set; }
        public Vector2 Scale { get; set; }
        public uint Field14 { get; set; }
        public uint Field18 { get; set; }
        public short FontSpacingCorrection { get; set; }
        public ushort Field1E { get; set; }
        public uint FontListOffset { get; set; }
        public string Characters { get; set; }
        public SWFontListV1 FontList { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Field00 = reader.Read<uint>();
            FontListIndex = reader.Read<uint>();
            CharacterListOffset = reader.Read<uint>();
            Scale = new Vector2(reader.Read<float>(), reader.Read<float>());
            Field14 = reader.Read<uint>();
            Field18 = reader.Read<uint>();
            FontSpacingCorrection = reader.Read<short>();
            Field1E = reader.Read<ushort>();
            FontListOffset = reader.Read<uint>();

            FontList = reader.ReadObjectAtOffset<SWFontListV1>(FontListOffset);
            reader.ReadAtOffset(CharacterListOffset, () => { Characters = reader.ReadString(StringBinaryFormat.NullTerminated); });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
