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
    public class SWProject
    {
        public string Name { get; set; }
        public ushort SceneCount { get; set; }
        public ushort Field06 { get; set; }
        public ushort TextureListCount { get; set; }
        public ushort FontListCount { get; set; }
        public uint SceneOffset { get; set; }
        public uint TextureListOffset { get; set; }
        public uint FontListOffset { get; set; }
        public SWCamera Camera { get; set; }
        public uint StartFrame { get; set; }
        public uint EndFrame { get; set; }
        public float FrameRate { get; set; }
        public uint Field5C { get; set; }
        public List<SWScene> Scenes { get; set; }
        public List<SWTextureList> TextureLists { get; set; }
        public List<SWFontList> FontLists { get; set; }

        public SWProject()
        {
            Camera = new SWCamera();
            Scenes = new List<SWScene>();
            TextureLists = new List<SWTextureList>();
            FontLists = new List<SWFontList>();
        }

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.ReadUInt32();
            Name = reader.ReadAbsoluteStringOffset(nameOffset);

            SceneCount = reader.ReadUInt16();
            Field06 = reader.ReadUInt16();
            TextureListCount = reader.ReadUInt16();
            FontListCount = reader.ReadUInt16();

            SceneOffset = reader.ReadUInt32();
            TextureListOffset = reader.ReadUInt32();
            FontListOffset = reader.ReadUInt32();

            Camera.Read(reader);

            StartFrame = reader.ReadUInt32();
            EndFrame = reader.ReadUInt32();
            FrameRate = reader.ReadSingle();
            Field5C = reader.ReadUInt32();

            reader.Seek(SceneOffset, SeekOrigin.Begin);
            for (int i = 0; i < SceneCount; i++)
            {
                SWScene scene = new SWScene();
                scene.Read(reader);

                Scenes.Add(scene);
            }

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
    }
}
