using System.Collections.Generic;
using Amicitia.IO.Binary;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWProjectNode : IBinarySerializable
    {
        public string Name { get; set; }
        public ushort SceneCount { get; set; }
        public ushort Field06 { get; set; }
        public ushort TextureListCount { get; set; }
        public ushort FontListCount { get; set; }
        public uint SceneOffset { get; set; }
        public uint TextureListOffset { get; set; }
        public uint FontListOffset { get; set; }
        public SWCamera Camera { get; set; } = new();
        public uint StartFrame { get; set; }
        public uint EndFrame { get; set; }
        public float FrameRate { get; set; }
        public uint Field5C { get; set; }
        public List<SWScene> Scenes { get; set; } = new();
        public List<SWTextureList> TextureLists { get; set; } = new();
        public List<SWFontList> FontLists { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Name = reader.ReadStringOffset(reader.Read<uint>(), true);

            SceneCount = reader.Read<ushort>();
            Field06 = reader.Read<ushort>();
            TextureListCount = reader.Read<ushort>();
            FontListCount = reader.Read<ushort>();

            SceneOffset = reader.Read<uint>();
            TextureListOffset = reader.Read<uint>();
            FontListOffset = reader.Read<uint>();

            Camera = reader.ReadObject<SWCamera>();

            StartFrame = reader.Read<uint>();
            EndFrame = reader.Read<uint>();
            FrameRate = reader.Read<float>();
            Field5C = reader.Read<uint>();

            reader.ReadAtOffset(SceneOffset, () =>
            {
                for (int i = 0; i < SceneCount; i++)
                    Scenes.Add(reader.ReadObject<SWScene>());
            }, true);

            reader.ReadAtOffset(TextureListOffset, () =>
            {
                for (int i = 0; i < TextureListCount; i++)
                    TextureLists.Add(reader.ReadObject<SWTextureList>());
            }, true);

            reader.ReadAtOffset(FontListOffset, () =>
            {
                for (int i = 0; i < FontListCount; i++)
                    FontLists.Add(reader.ReadObject<SWFontList>());
            }, true);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
