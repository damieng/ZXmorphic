using System;

using Morphic.Core.Bus;

namespace Spectrum.Custom
{
	public class Spectrum48KULA : IBus16Bit
    {
        #region Construction

        private byte[] io = new byte[256];

        public Spectrum48KULA()
        {
        }

        #endregion

        #region I16BitBus IO interface

        public Byte ReadByte(UInt16 address)
		{
            return io[address % 256];
		}

		public void WriteByte(UInt16 address, Byte data)
		{
            io[address % 256] = data;
		}

		public UInt16 ReadWord(UInt16 address)
		{
            return (UInt16) ((int)io[address % 256] * 256 + (int)io[++address % 256]);
		}

		public void WriteWord(UInt16 address, UInt16 data)
		{
            io[address % 256] = (byte) (data / 256);
            io[++address % 256] = (byte) (data % 256);
		}

		#endregion
    }
}