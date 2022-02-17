using System.Collections.Generic;
using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWSceneV2 : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint Field0C { get; set; }
        public uint Flags { get; set; }
        public uint LayerCount { get; set; }
        public ulong LayerOffset { get; set; }
        public ulong CameraCount { get; set; }
        public ulong CameraOffset { get; set; }
        public uint BackgroundColor { get; set; }
        public Vector2 FrameSize { get; set; }
        public ulong Field3C { get; set; }
        public List<SWLayerV2> Layers { get; set; } = new();
        public List<SWCameraV2> Cameras { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });
            ID = reader.Read<uint>();
            Field0C = reader.Read<uint>();
            Flags = reader.Read<uint>();

            LayerCount = reader.Read<uint>();
            LayerOffset = reader.Read<ulong>();

            CameraCount = reader.Read<ulong>();
            CameraOffset = reader.Read<ulong>();

            BackgroundColor = reader.Read<uint>();

            FrameSize = new Vector2(reader.Read<float>(), reader.Read<float>());
            Field3C = reader.Read<ulong>();

            reader.ReadAtOffset((long)LayerOffset, () =>
            {
                for (int i = 0; i < LayerCount; i++)
                    Layers.Add(reader.ReadObject<SWLayerV2>());
            });

            reader.ReadAtOffset((long)CameraOffset, () =>
            {
                for (ulong i = 0; i < CameraCount; i++)
                    Cameras.Add(reader.ReadObject<SWCameraV2>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
