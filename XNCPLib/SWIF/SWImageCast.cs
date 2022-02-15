using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWImageCast
    {
        public class SWPatternInfo
        {
            public ushort TextureListIndex { get; set; }
            public ushort TextureMapIndex { get; set; }
            public ushort SpriteIndex { get; set; }

            public void Read(BinaryObjectReader reader)
            {
                TextureListIndex = reader.ReadUInt16();
                TextureMapIndex = reader.ReadUInt16();
                SpriteIndex = reader.ReadUInt16();
            }
        }

        public class SWFontInfo
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
            public SWFontList FontList { get; set; }

            public SWFontInfo()
            {
                Scale = new Vector2(1.0f, 1.0f);
                FontList = new SWFontList();
            }

            public void Read(BinaryObjectReader reader)
            {
                Field00 = reader.ReadUInt32();
                FontListIndex = reader.ReadUInt32();
                CharacterListOffset = reader.ReadUInt32();
                Scale = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                Field14 = reader.ReadUInt32();
                Field18 = reader.ReadUInt32();
                FontSpacing = reader.ReadInt16();
                Field1E = reader.ReadUInt16();
                FontListOffset = reader.ReadUInt32();

                reader.PushOffsetOrigin();

                reader.Seek(FontListOffset, SeekOrigin.Begin);
                FontList.Read(reader);

                reader.Seek(CharacterListOffset, SeekOrigin.Begin);
                Characters = reader.ReadString(StringBinaryFormat.NullTerminated);

                reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
                reader.PopOffsetOrigin();
            }
        }

        public enum EFlags
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
        public List<SWPatternInfo> PatternInfoList { get; set; }
        public SWFontInfo FontInfo { get; set; }

        public SWImageCast()
        {
            PatternInfoList = new List<SWPatternInfo>();
            FontInfo = new SWFontInfo();
            AnchorPoint = new Vector2();
        }

        public void Read(BinaryObjectReader reader)
        {
            Flags = (EFlags)reader.ReadUInt32();
            Width = reader.ReadSingle();
            Height = reader.ReadSingle();
            AnchorPoint = new Vector2(reader.ReadSingle(), reader.ReadSingle());

            GradientTopLeft = reader.ReadUInt32();
            GradientBottomLeft = reader.ReadUInt32();
            GradientTopRight = reader.ReadUInt32();
            GradientBottomRight = reader.ReadUInt32();

            Field24 = reader.ReadInt16();
            Field26 = reader.ReadInt16();

            PatternInfoCount = reader.ReadUInt16();
            Field2A = reader.ReadUInt16();
            PatternInfoOffset = reader.ReadUInt32();

            Field30 = reader.ReadUInt32();
            FontInfoOffset = reader.ReadUInt32();
            Field38 = reader.ReadUInt32();
            Field3C = reader.ReadUInt32();

            reader.PushOffsetOrigin();
            reader.Seek(PatternInfoOffset, SeekOrigin.Begin);
            for (int i = 0; i < PatternInfoCount; i++)
            {
                SWPatternInfo patternInfo = new SWPatternInfo();
                patternInfo.Read(reader);

                PatternInfoList.Add(patternInfo);
            }

            if ((Flags & EFlags.eFlags_UseFont) != 0)
            {
                reader.Seek(FontInfoOffset, SeekOrigin.Begin);
                FontInfo.Read(reader);
            }

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }
    }
}