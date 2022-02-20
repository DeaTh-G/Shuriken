using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimationTrack : IBinarySerializable
    {
        public ushort AnimationType { get; set; }
        public ushort KeyCount { get; set; }
        public uint Flags { get; set; }
        public uint StartFrame { get; set; }
        public uint EndFrame { get; set; }
        public uint TrackOffset { get; set; }
        public List<ISWKey> Keys { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            AnimationType = reader.Read<ushort>();
            KeyCount = reader.Read<ushort>();
            Flags = reader.Read<uint>();
            StartFrame = reader.Read<uint>();
            EndFrame = reader.Read<uint>();
            TrackOffset = reader.Read<uint>();

            reader.ReadAtOffset(TrackOffset, () =>
            {
                for (int i = 0; i < KeyCount; i++)
                {
                    switch (Flags & 3)
                    {
                        case 0:
                            Keys.Add(reader.ReadObject<SWKeyConstant, uint>(Flags));
                            break;
                        case 1:
                            Keys.Add(reader.ReadObject<SWKeyLinear, uint>(Flags));
                            break;
                        case 2:
                            Keys.Add(reader.ReadObject<SWKeyHermite, uint>(Flags));
                            break;
                        case 3:
                            Keys.Add(reader.ReadObject<SWKeyIndividual, uint>(Flags));
                            break;
                        default:
                            break;
                    }
                }
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
