using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWProjectNodeV2 : IBinarySerializable
    {
        public string Name { get; set; }
        public ushort SceneCount { get; set; }
        public ushort Field06 { get; set; }
        public ushort TextureListCount { get; set; }
        public ushort FontListCount { get; set; }
        public ulong SceneOffset { get; set; }
        public ulong TextureListOffset { get; set; }
        public ulong FontListOffset { get; set; }
        public SWCameraV2 Camera { get; set; } = new();
        public uint StartFrame { get; set; }
        public uint EndFrame { get; set; }
        public float FrameRate { get; set; }
        public ulong Field5C { get; set; }
        public List<SWSceneV2> Scenes { get; set; } = new();
        public List<SWTextureListV2> TextureLists { get; set; } = new();
        public List<SWFontListV2> FontLists { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            SceneCount = reader.Read<ushort>();
            Field06 = reader.Read<ushort>();
            TextureListCount = reader.Read<ushort>();
            FontListCount = reader.Read<ushort>();

            SceneOffset = reader.Read<ulong>();
            TextureListOffset = reader.Read<ulong>();
            FontListOffset = reader.Read<ulong>();

            Camera = reader.ReadObject<SWCameraV2>();

            StartFrame = reader.Read<uint>();
            EndFrame = reader.Read<uint>();
            FrameRate = reader.Read<float>();
            Field5C = reader.Read<ulong>();

            reader.ReadAtOffset((long)SceneOffset, () =>
            {
                for (int i = 0; i < SceneCount; i++)
                    Scenes.Add(reader.ReadObject<SWSceneV2>());
            });

            reader.ReadAtOffset((long)TextureListOffset, () =>
            {
                for (int i = 0; i < TextureListCount; i++)
                    TextureLists.Add(reader.ReadObject<SWTextureListV2>());
            });

            reader.ReadAtOffset((long)FontListOffset, () =>
            {
                for (int i = 0; i < FontListCount; i++)
                    FontLists.Add(reader.ReadObject<SWFontListV2>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
