using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast.ImageCast
{
    public class SWFontInfoV2 : ISWFontInfo
    {
        public uint Field00 { get; set; }
        public uint FontListIndex { get; set; }
        public long CharacterListOffset { get; set; }
        public Vector2 Scale { get; set; }
        public uint Field18 { get; set; }
        public uint Field1C { get; set; }
        public short FontSpacingCorrection { get; set; }
        public ushort Field22 { get; set; }
        public uint Field24 { get; set; }
        public long FontListOffset { get; set; }
        public string Characters { get; set; }
        public ISWFontList FontList { get; set; } = new SWFontListV2();

        public void Read(BinaryObjectReader reader)
        {
            Field00 = reader.Read<uint>();
            FontListIndex = reader.Read<uint>();
            CharacterListOffset = reader.ReadOffsetValue();
            Scale = new Vector2(reader.Read<float>(), reader.Read<float>());
            Field18 = reader.Read<uint>();
            Field1C = reader.Read<uint>();
            FontSpacingCorrection = reader.Read<short>();
            Field22 = reader.Read<ushort>();
            Field24 = reader.Read<uint>();
            FontListOffset = reader.ReadOffsetValue();

            FontList = reader.ReadObjectAtOffset<SWFontListV2>(FontListOffset);
            reader.ReadAtOffset(CharacterListOffset, () => { Characters = reader.ReadString(StringBinaryFormat.NullTerminated); });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
