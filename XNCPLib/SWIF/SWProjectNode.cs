using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWProjectNode : IBinarySerializable<uint>
    {
        public string Name { get; set; }
        public ushort SceneCount { get; set; }
        public ushort Field06 { get; set; }
        public ushort TextureListCount { get; set; }
        public ushort FontListCount { get; set; }
        public long SceneOffset { get; set; }
        public long TextureListOffset { get; set; }
        public long FontListOffset { get; set; }
        public ISWCamera Camera { get; set; }
        public uint StartFrame { get; set; }
        public uint EndFrame { get; set; }
        public float FrameRate { get; set; }
        public long Field5C { get; set; }
        public List<ISWScene> Scenes { get; set; } = new();
        public List<ISWTextureList> TextureLists { get; set; } = new();
        public List<ISWFontList> FontLists { get; set; } = new();

        public void Read(BinaryObjectReader reader, uint version)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            SceneCount = reader.Read<ushort>();
            Field06 = reader.Read<ushort>();
            TextureListCount = reader.Read<ushort>();
            FontListCount = reader.Read<ushort>();

            SceneOffset = reader.ReadOffsetValue();
            TextureListOffset = reader.ReadOffsetValue();
            FontListOffset = reader.ReadOffsetValue();

            if (version == 1)
                Camera = reader.ReadObject<SWCameraV1>();
            else if (version == 2)
                Camera = reader.ReadObject<SWCameraV2>();

            StartFrame = reader.Read<uint>();
            EndFrame = reader.Read<uint>();
            FrameRate = reader.Read<float>();
            Field5C = reader.ReadOffsetValue();

            reader.ReadAtOffset(SceneOffset, () =>
            {
                for (int i = 0; i < SceneCount; i++)
                {
                    if (version == 1)
                        Scenes.Add(reader.ReadObject<SWSceneV1>());
                    else if (version == 2)
                        Scenes.Add(reader.ReadObject<SWSceneV2>());
                }
            });

            reader.ReadAtOffset(TextureListOffset, () =>
            {
                for (int i = 0; i < TextureListCount; i++)
                {
                    if (version == 1)
                        TextureLists.Add(reader.ReadObject<SWTextureListV1>());
                    else if (version == 2)
                        TextureLists.Add(reader.ReadObject<SWTextureListV2>());
                }
            });

            reader.ReadAtOffset(FontListOffset, () =>
            {
                for (int i = 0; i < FontListCount; i++)
                {
                    if (version == 1)
                        FontLists.Add(reader.ReadObject<SWFontListV1>());
                    else if (version == 2)
                        FontLists.Add(reader.ReadObject<SWFontListV2>());
                }
            });
        }

        public void Write(BinaryObjectWriter writer, uint version) { }
    }
}
