using Amicitia.IO.Binary;

namespace XNCPLib.SWIF.Cast.ImageCast
{
    public struct SWPatternInfo : IBinarySerializable
    {
        public ushort TextureListIndex;
        public ushort TextureMapIndex;
        public ushort SpriteIndex;

        public void Read(BinaryObjectReader reader)
        {
            reader.Read(out TextureListIndex);
            reader.Read(out TextureMapIndex);
            reader.Read(out SpriteIndex);
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
