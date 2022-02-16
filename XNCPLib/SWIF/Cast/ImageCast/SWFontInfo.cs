using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using System.IO;
using System.Numerics;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF.Cast.ImageCast
{
    public class SWFontInfo : IBinarySerializable
    {
        public uint Field00 { get; set; }
        public uint FontListIndex { get; set; }
        public uint CharacterListOffset { get; set; }
        public Vector2 Scale { get; set; }
        public uint Field14 { get; set; }
        public uint Field18 { get; set; }
        public short FontSpacing { get; set; }
        public ushort Field1E { get; set; }
        public uint FontListOffset { get; set; }
        public string Characters { get; set; }
        public SWFontList FontList { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Field00 = reader.Read<uint>();
            FontListIndex = reader.Read<uint>();
            CharacterListOffset = reader.Read<uint>();
            Scale = new Vector2(reader.Read<float>(), reader.Read<float>());
            Field14 = reader.Read<uint>();
            Field18 = reader.Read<uint>();
            FontSpacing = reader.Read<short>();
            Field1E = reader.Read<ushort>();
            FontListOffset = reader.Read<uint>();

            reader.PushOffsetOrigin();
            reader.Seek(FontListOffset, SeekOrigin.Begin);
            FontList = reader.ReadObject<SWFontList>();

            reader.Seek(CharacterListOffset, SeekOrigin.Begin);
            Characters = reader.ReadString(StringBinaryFormat.NullTerminated);

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
