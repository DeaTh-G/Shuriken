using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Amicitia.IO.Binary;

namespace XNCPLib.SWIF
{
    public class SWCamera
    {
        public StringOffset Name { get; set; }
        public uint ID;
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public uint Flags { get; set; }
        public float RangeIn { get; set; }
        public float RangeOut { get; set; }
        public uint Field2C { get; set; }
        public uint Field30 { get; set; }

        public SWCamera()
        {
            Name = new StringOffset();
            Position = new Vector3(0.0f, 1.0f, 0.0f);
            LookAt = new Vector3(0.0f, 1.0f, 0.0f);
        }

        public void Read(BinaryObjectReader reader)
        {
            Name.Read(reader);
            ID = reader.ReadUInt32();

            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            LookAt = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            Flags = reader.ReadUInt32();
            RangeIn = reader.ReadSingle();
            RangeOut = reader.ReadSingle();

            Field2C = reader.ReadUInt32();
            Field30 = reader.ReadUInt32();
        }
    }
}
