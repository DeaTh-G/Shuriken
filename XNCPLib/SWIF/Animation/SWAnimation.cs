using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimation : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint AnimationLinkCount { get; set; }
        public uint FrameCount { get; set; }
        public uint AnimationLinkOffset { get; set; }
        public uint Field14 { get; set; }
        public byte Field18 { get; set; }
        public byte Field19 { get; set; }
        public byte Field1A { get; set; }
        public byte Field1B { get; set; }
        public List<SWAnimationLink> AnimationLinks { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            ID = reader.Read<uint>();

            AnimationLinkCount = reader.Read<uint>();
            FrameCount = reader.Read<uint>();
            AnimationLinkOffset = reader.Read<uint>();

            Field14 = reader.Read<uint>();

            Field18 = reader.Read<byte>();
            Field19 = reader.Read<byte>();
            Field1A = reader.Read<byte>();
            Field1B = reader.Read<byte>();

            reader.ReadAtOffset(AnimationLinkOffset, () =>
            {
                for (int i = 0; i < AnimationLinkCount; i++)
                    AnimationLinks.Add(reader.ReadObject<SWAnimationLink>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
