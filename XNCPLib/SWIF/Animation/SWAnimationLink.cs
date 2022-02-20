using Amicitia.IO.Binary;
using System.Collections.Generic;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimationLink : IBinarySerializable
    {
        public ushort CastID { get; set; }
        public ushort TimelinesCount { get; set; }
        public uint TimelinesOffset { get; set; }
        public List<SWAnimationTimeline> Timelines { get; set; } = new(); 

        public void Read(BinaryObjectReader reader)
        {
            CastID = reader.Read<ushort>();

            TimelinesCount = reader.Read<ushort>();
            TimelinesOffset = reader.Read<uint>();

            reader.ReadAtOffset(TimelinesOffset, () =>
            {
                for (int i = 0; i < TimelinesCount; i++)
                    Timelines.Add(reader.ReadObject<SWAnimationTimeline>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
