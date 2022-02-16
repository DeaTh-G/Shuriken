using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWProject : IBinarySerializable
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
            uint nameOffset = reader.Read<uint>();
            Name = reader.ReadStringOffset(nameOffset, true);

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

            reader.Seek(SceneOffset, SeekOrigin.Begin);
            for (int i = 0; i < SceneCount; i++)
                Scenes.Add(reader.ReadObject<SWScene>());

            reader.Seek(TextureListOffset, SeekOrigin.Begin);
            for (int i = 0; i < TextureListCount; i++)
            {
                SWTextureList textureList = new SWTextureList();
                textureList.Read(reader);

                TextureLists.Add(textureList);
            }

            reader.Seek(FontListOffset, SeekOrigin.Begin);
            for (int i = 0; i < FontListCount; i++)
            {
                SWFontList fontList = new SWFontList();
                fontList.Read(reader);

                FontLists.Add(fontList);
            }
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
