﻿using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public struct SWFontMapping : IBinarySerializable
    {
        public ushort Character;
        public short TextureListIndex;
        public short TextureMapIndex;
        public short SpriteIndex;

        public void Read(BinaryObjectReader reader)
        {
            reader.Read(out Character);
            reader.Read(out TextureListIndex);
            reader.Read(out TextureMapIndex);
            reader.Read(out SpriteIndex);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
