using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWSubImage : IBinarySerializable
    {
        public Vector2 TopLeft { get; set; }
        public Vector2 BottomRight { get; set; }

        public void Read(BinaryObjectReader reader)
        {
            TopLeft = new Vector2(reader.Read<float>(), reader.Read<float>());
            BottomRight = new Vector2(reader.Read<float>(), reader.Read<float>());
        }

        public void Write(BinaryObjectWriter writer) { }
    }
}
