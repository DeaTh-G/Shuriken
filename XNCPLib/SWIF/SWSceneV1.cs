using System.Collections.Generic;
using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWSceneV1 : ISWScene
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint Flags { get; set; }
        public uint LayerCount { get; set; }
        public long LayerOffset { get; set; }
        public long CameraCount { get; set; }
        public ushort Field16 { get; set; }
        public long CameraOffset { get; set; }
        public uint BackgroundColor { get; set; }
        public Vector2 FrameSize { get; set; } = new();
        public long Field3C { get; set; }
        public List<ISWLayer> Layers { get; set; } = new();
        public List<ISWCamera> Cameras { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            ID = reader.Read<uint>();
            Flags = reader.Read<uint>();

            LayerCount = reader.Read<uint>();
            LayerOffset = reader.ReadOffsetValue();

            CameraCount = reader.Read<ushort>();
            Field16 = reader.Read<ushort>();
            CameraOffset = reader.ReadOffsetValue();

            BackgroundColor = reader.Read<uint>();

            FrameSize = new Vector2(reader.Read<float>(), reader.Read<float>());
            Field3C = reader.ReadOffsetValue();

            reader.ReadAtOffset(LayerOffset, () =>
            {
                for (int i = 0; i < LayerCount; i++)
                    Layers.Add(reader.ReadObject<SWLayerV1>());
            });

            reader.ReadAtOffset(CameraOffset, () =>
            {
                for (int i = 0; i < CameraCount; i++)
                    Cameras.Add(reader.ReadObject<SWCameraV1>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
