using System.Collections.Generic;
using System.Numerics;
using Amicitia.IO.Binary;
using XNCPLib.SWIF.Cast.ImageCast;

namespace XNCPLib.SWIF.Cast
{
    public class SWImageCastV1 : IBinarySerializable
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
        public ushort Field2A { get; set; }
        public uint PatternInfoOffset { get; set; }
        public uint Field30 { get; set; }
        public uint FontInfoOffset { get; set; }
        public uint Field38 { get; set; }
        public uint Field3C { get; set; }
        public List<SWPatternInfo> PatternInfoList { get; set; } = new();
        public SWFontInfoV1 FontInfo { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Flags = reader.Read<EFlags>();
            Width = reader.Read<float>();
            Height = reader.Read<float>();
            AnchorPoint = new Vector2(reader.Read<float>(), reader.Read<float>());

            for (int i = 0; i < 4; i++)
                GradientTopLeft |= (uint)(reader.Read<byte>() << 8 * i);

            for (int i = 0; i < 4; i++)
                GradientBottomLeft |= (uint)(reader.Read<byte>() << 8 * i);

            for (int i = 0; i < 4; i++)
                GradientTopRight |= (uint)(reader.Read<byte>() << 8 * i);

            for (int i = 0; i < 4; i++)
                GradientBottomRight |= (uint)(reader.Read<byte>() << 8 * i);

            Field24 = reader.Read<short>();
            Field26 = reader.Read<short>();

            PatternInfoCount = reader.Read<ushort>();
            Field2A = reader.Read<ushort>();
            PatternInfoOffset = reader.Read<uint>();

            Field30 = reader.Read<uint>();
            FontInfoOffset = reader.Read<uint>();
            Field38 = reader.Read<uint>();
            Field3C = reader.Read<uint>();

            reader.ReadAtOffset(PatternInfoOffset, () =>
            {
                for (int i = 0; i < PatternInfoCount; i++)
                    PatternInfoList.Add(reader.ReadObject<SWPatternInfo>());
            });

            if ((Flags & (EFlags)0xFFF) == EFlags.eFlags_UseFont)
                FontInfo = reader.ReadObjectAtOffset<SWFontInfoV1>(FontInfoOffset);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}