using System.Collections.Generic;
using System.Numerics;
using Amicitia.IO.Binary;
using XNCPLib.SWIF.Cast.ImageCast;

namespace XNCPLib.SWIF.Cast
{
    public interface ISWImageCast : IBinarySerializable
    {
        public enum EFlags : uint
        {
            eFlags_Transparent = 1,
            eFlags_InvertColors = 2,
            eFlags_Unk3 = 4,
            eFlags_FlipHorizontally = 0x10,
            eFlags_FlipVertically = 0x20,
            eFlags_RotateLeft = 0x40,
            eFlags_FlipHorizontallyAndVertically = 0x80,
            eFlags_UseFont = 0x100,
            eFlags_AntiAliasing = 0x2000,
            eFlags_AnchorBottomRight = 0x40000,
            eFlags_AnchorBottom = 0x80000,
            eFlags_AnchorBottomLeft = 0x100000,
            eFlags_AnchorRight = 0x180000,
            eFlags_AnchorCenter = 0x200000,
            eFlags_AnchorLeft = 0x280000,
            eFlags_AnchorTopRight = 0x300000,
            eFlags_AnchorTop = 0x380000,
            eFlags_AnchorTopLeft = 0x400000,
        }

        public EFlags Flags { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Vector2 AnchorPoint { get; set; }
        public uint GradientTopLeft { get; set; }
        public uint GradientBottomLeft { get; set; }
        public uint GradientTopRight { get; set; }
        public uint GradientBottomRight { get; set; }
        public short Field24 { get; set; }
        public short Field26 { get; set; }
        public ushort PatternInfoCount { get; set; }
        public short Field2A { get; set; }
        public long PatternInfoOffset { get; set; }
        public long Field38 { get; set; }
        public long FontInfoOffset { get; set; }
        public long Field48 { get; set; }
        public long Field50 { get; set; }
        public List<SWPatternInfo> PatternInfoList { get; set; }
        public ISWFontInfo FontInfo { get; set; }
    }
}
