using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimationTimeline : IBinarySerializable
    {
        public ushort Field00 { get; set; }
        public ushort TrackCount { get; set; }
        public uint Flags { get; set; }
        public uint StartFrame { get; set; }
        public uint EndFrame { get; set; }
        public uint TrackOffset { get; set; }
        public List<ISWAnimationTrack> Tracks { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            Field00 = reader.Read<ushort>();
            TrackCount = reader.Read<ushort>();
            Flags = reader.Read<uint>();
            StartFrame = reader.Read<uint>();
            EndFrame = reader.Read<uint>();
            TrackOffset = reader.Read<uint>();

            reader.ReadAtOffset(TrackOffset, () =>
            {
                for (int i = 0; i < TrackCount; i++)
                {
                    switch (Flags & 3)
                    {
                        case 0:
                            Tracks.Add(reader.ReadObject<SWAnimationTrackConstant, uint>(Flags));
                            break;
                        case 1:
                            Tracks.Add(reader.ReadObject<SWAnimationTrackLinear, uint>(Flags));
                            break;
                        case 2:
                            Tracks.Add(reader.ReadObject<SWAnimationTrackHermite, uint>(Flags));
                            break;
                        case 3:
                            Tracks.Add(reader.ReadObject<SWAnimationTrackIndividual, uint>(Flags));
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
