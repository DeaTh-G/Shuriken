﻿using System.Collections.Generic;
using System.Numerics;
using Amicitia.IO.Binary;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF
{
    public class SWScene : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint Flags { get; set; }
        public uint LayerCount { get; set; }
        public uint LayerOffset { get; set; }
        public ushort CameraCount { get; set; }
        public ushort Field16 { get; set; }
        public uint CameraOffset { get; set; }
        public uint BackgroundColor { get; set; }
        public Vector2 FrameSize { get; set; }
        public uint Field28 { get; set; }
        public List<SWLayer> Layers { get; set; } = new();
        public List<SWCamera> Cameras { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Name = reader.ReadStringOffset(reader.Read<uint>(), true);
            ID = reader.Read<uint>();
            Flags = reader.Read<uint>();

            LayerCount = reader.Read<uint>();
            LayerOffset = reader.Read<uint>();

            CameraCount = reader.Read<ushort>();
            Field16 = reader.Read<ushort>();
            CameraOffset = reader.Read<uint>();

            BackgroundColor = reader.Read<uint>();

            FrameSize = new Vector2(reader.Read<float>(), reader.Read<float>());
            Field28 = reader.Read<uint>();

            reader.ReadAtOffset(LayerOffset, () =>
            {
                for (int i = 0; i < LayerCount; i++)
                    Layers.Add(reader.ReadObject<SWLayer>());
            });

            reader.ReadAtOffset(CameraOffset, () =>
            {
                for (int i = 0; i < CameraCount; i++)
                    Cameras.Add(reader.ReadObject<SWCamera>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
