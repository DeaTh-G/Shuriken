using System.Collections.Generic;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimationV2 : IBinarySerializable
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint AnimationLinkCount { get; set; }
        public ulong Field10 { get; set; }
        public ulong AnimationLinkOffset { get; set; }
        public uint Field20 { get; set; }
        public byte Field24 { get; set; }
        public byte Field25 { get; set; }
        public byte Field26 { get; set; }
        public byte Field27 { get; set; }
        public ulong Field28 { get; set; }
        public List<SWAnimationLink> AnimationLinks { get; set; } = new();

        public void Read(BinaryObjectReader reader)
        {
            reader.ReadOffset(() => { Name = reader.ReadString(StringBinaryFormat.NullTerminated); });

            ID = reader.Read<uint>();

            AnimationLinkCount = reader.Read<uint>();
            Field10 = reader.Read<ulong>();
            AnimationLinkOffset = reader.Read<ulong>();

            Field20 = reader.Read<uint>();

            Field24 = reader.Read<byte>();
            Field25 = reader.Read<byte>();
            Field26 = reader.Read<byte>();
            Field27 = reader.Read<byte>();
            Field28 = reader.Read<ulong>();

            reader.ReadAtOffset((long)AnimationLinkOffset, () =>
            {
                for (int i = 0; i < AnimationLinkCount; i++)
                    AnimationLinks.Add(reader.ReadObject<SWAnimationLink>());
            });
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
