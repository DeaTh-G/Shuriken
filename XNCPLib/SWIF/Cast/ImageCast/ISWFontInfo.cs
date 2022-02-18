using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast.ImageCast
{
    public interface ISWFontInfo : IBinarySerializable
    {
        public uint Field00 { get; set; }
        public uint FontListIndex { get; set; }
        public long CharacterListOffset { get; set; }
        public Vector2 Scale { get; set; }
        public uint Field18 { get; set; }
        public uint Field1C { get; set; }
        public short FontSpacingCorrection { get; set; }
        public ushort Field22 { get; set; }
        public long FontListOffset { get; set; }
        public string Characters { get; set; }
        public ISWFontList FontList { get; set; }
    }
}
