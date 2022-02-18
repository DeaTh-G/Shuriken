using System.Collections.Generic;
using System.Numerics;
using Amicitia.IO.Binary;
using XNCPLib.SWIF.Cast.ImageCast;

namespace XNCPLib.SWIF.Cast
{
    public class SWImageCastV1 : ISWImageCast
    {
        public ISWImageCast.EFlags Flags { get; set; }
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
        public List<SWPatternInfo> PatternInfoList { get; set; } = new();
        public ISWFontInfo FontInfo { get; set; } = new SWFontInfoV1();

        public void Read(BinaryObjectReader reader)
        {
            Flags = reader.Read<ISWImageCast.EFlags>();
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
            Field2A = reader.Read<short>();
            PatternInfoOffset = reader.ReadOffsetValue();

            Field38 = reader.ReadOffsetValue();
            FontInfoOffset = reader.ReadOffsetValue();
            Field48 = reader.ReadOffsetValue();
            Field50 = reader.ReadOffsetValue();

            reader.ReadAtOffset(PatternInfoOffset, () =>
            {
                for (int i = 0; i < PatternInfoCount; i++)
                    PatternInfoList.Add(reader.ReadObject<SWPatternInfo>());
            });

            if ((Flags & (ISWImageCast.EFlags)0xFFF) == ISWImageCast.EFlags.eFlags_UseFont)
                FontInfo = reader.ReadObjectAtOffset<SWFontInfoV1>(FontInfoOffset);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}