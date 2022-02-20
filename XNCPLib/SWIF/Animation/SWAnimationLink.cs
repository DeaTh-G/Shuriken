using Amicitia.IO.Binary;
using System.Collections.Generic;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimationLink : IBinarySerializable
    {
        public ushort CastID { get; set; }
        public ushort TrackCount { get; set; }
        public uint TrackOffset { get; set; }
        public List<SWAnimationTrack> Tracks { get; set; } = new(); 

        public void Read(BinaryObjectReader reader)
        {
            CastID = reader.Read<ushort>();

            TrackCount = reader.Read<ushort>();
            TrackOffset = reader.Read<uint>();

            reader.ReadAtOffset(TrackOffset, () =>
            {
                for (int i = 0; i < TrackCount; i++)
                    Tracks.Add(reader.ReadObject<SWAnimationTrack>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
