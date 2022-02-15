﻿using System;
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
    public class SWScene
    {
        public enum EFlags
        {
            eFlags_Hidden = 1
        }

        public string Name { get; set; }
        public uint ID { get; set; }
        public EFlags Flags { get; set; }
        public uint LayerCount { get; set; }
        public uint LayerOffset { get; set; }
        public ushort CameraCount { get; set; }
        public ushort Field16 { get; set; }
        public uint CameraOffset { get; set; }
        public uint BackgroundColor { get; set; }
        public Vector2 FrameSize { get; set; }
        public uint Field28 { get; set; }
        public List<SWLayer> Layers { get; set; }
        public List<SWCamera> Cameras { get; set; }

        public SWScene()
        {
            FrameSize = new Vector2(1280, 720);
            Layers = new List<SWLayer>();
            Cameras = new List<SWCamera>();
        }

        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.ReadUInt32();
            Name = reader.ReadAbsoluteStringOffset(nameOffset);
            ID = reader.ReadUInt32();
            Flags = (EFlags)reader.ReadUInt32();

            LayerCount = reader.ReadUInt32();
            LayerOffset = reader.ReadUInt32();

            CameraCount = reader.ReadUInt16();
            Field16 = reader.ReadUInt16();
            CameraOffset = reader.ReadUInt32();

            BackgroundColor = reader.ReadUInt32();

            FrameSize = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            Field28 = reader.ReadUInt32();

            reader.PushOffsetOrigin();
            reader.Seek(LayerOffset, SeekOrigin.Begin);
            for (int i = 0; i < LayerCount; i++)
            {
                SWLayer layer = new SWLayer();
                layer.Read(reader);

                Layers.Add(layer);
            }

            reader.Seek(CameraOffset, SeekOrigin.Begin);
            for (int i = 0; i < CameraCount; i++)
            {
                SWCamera camera = new SWCamera();
                camera.Read(reader);

                Cameras.Add(camera);
            }

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }
    }
}
