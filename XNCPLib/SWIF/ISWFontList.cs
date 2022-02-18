using Amicitia.IO.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNCPLib.SWIF
{
    public interface ISWFontList : IBinarySerializable
    {
        public string Name { get; set; }
        public uint Field08 { get; set; }
        public uint Field0C { get; set; }
        public ushort FontMappingCount { get; set; }
        public ushort Field14 { get; set; }
        public long FontMappingOffset { get; set; }
        public uint Field20 { get; set; }
        public uint Field24 { get; set; }
        public List<SWFontMapping> FontMappings { get; set; }
    }
}
