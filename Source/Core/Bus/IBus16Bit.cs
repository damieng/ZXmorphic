using System;

namespace Morphic.Core.Bus
{
	public interface IBus16Bit
	{
		Byte ReadByte(UInt16 address);
		void WriteByte(UInt16 address, Byte data);

		UInt16 ReadWord(UInt16 address);
		void WriteWord(UInt16 address, UInt16 data);
	}
}
