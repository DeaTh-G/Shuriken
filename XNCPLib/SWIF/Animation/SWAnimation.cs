using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using XNCPLib.Extensions;

namespace XNCPLib.SWIF.Animation
{
    public class SWAnimation
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint AnimationLinkCount { get; set; }
        public uint Field0C { get; set; }
        public uint AnimationLinkOffset { get; set; }
        public uint Field14 { get; set; }
        public byte Field18 { get; set; }
        public byte Field19 { get; set; }
        public byte Field1A { get; set; }
        public byte Field1B { get; set; }
        public List<SWAnimationLink> AnimationLinks { get; set; }

        public SWAnimation()
        {
            AnimationLinks = new List<SWAnimationLink>();
        }
        public void Read(BinaryObjectReader reader)
        {
            uint nameOffset = reader.ReadUInt32();
            Name = reader.ReadAbsoluteStringOffset(nameOffset);

            ID = reader.ReadUInt32();

            AnimationLinkCount = reader.ReadUInt32();
            Field0C = reader.ReadUInt32();
            AnimationLinkOffset = reader.ReadUInt32();

            Field14 = reader.ReadUInt32();

            Field18 = reader.ReadByte();
            Field19 = reader.ReadByte();
            Field1A = reader.ReadByte();
            Field1B = reader.ReadByte();

            reader.PushOffsetOrigin();

            reader.Seek(AnimationLinkOffset, SeekOrigin.Begin);
            for (int i = 0; i < AnimationLinkCount; i++)
            {
                SWAnimationLink animationLink = new SWAnimationLink();
                animationLink.Read(reader);

                AnimationLinks.Add(animationLink);
            }

            reader.Seek(reader.GetOffsetOrigin(), SeekOrigin.Begin);
            reader.PopOffsetOrigin();
        }
    }
}
