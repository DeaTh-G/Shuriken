using Amicitia.IO.Binary;
using System.Runtime.InteropServices;

namespace XNCPLib.Common
{
	[StructLayout(LayoutKind.Sequential, Size = BinarySize)]
	public struct ChunkHeader : IBinarySerializable
	{
		private const int BinarySize = 8;
		public uint Signature;
		public uint Size;

		public void Read(BinaryObjectReader reader)
		{
			reader.ReadNative(out Signature);
			reader.Read(out Size);
		}

		public void Write(BinaryObjectWriter writer) { }
	}
}