#region Imports

using System;
using System.IO;

using Morphic.Core.Bus;
using Morphic.Core.Memory;
using Morphic.Core.Primitive;

#endregion

namespace Spectrum.Memory
{
	public class Memory48K : IBus16Bit
	{
		#region Instance variables

		public PagedMemory pagedMemory;

		#endregion

		#region Construction

		public Memory48K()
		{
			Reset();
		}

		#endregion

		#region Methods

		public void Reset()
		{
			pagedMemory = new PagedMemory(4, 16384, 4);
			pagedMemory.AllPages[0].ReadOnly = true;
		}

		#endregion

		#region I16BitBus interface

		public Byte ReadByte(UInt16 address)
		{
			return pagedMemory.ReadByte(address);
		}

		public void WriteByte(UInt16 address, Byte data)
		{
			pagedMemory.WriteByte(address, data, false);
		}

		public UInt16 ReadWord(UInt16 address)
		{
			return pagedMemory.ReadWord(address);
		}

		public void WriteWord(UInt16 address, UInt16 data)
		{
			pagedMemory.WriteWord(address, data, false);
		}

		public int Load(UInt16 startAddress, string path)
		{
			FileStream fileStream = File.OpenRead(path);
			UInt16 address = startAddress;
			Int32 nextByte = fileStream.ReadByte();
			while (nextByte != -1) {
				pagedMemory.WriteByte(address++, (Byte)nextByte, true);
				nextByte = fileStream.ReadByte();
			}
			fileStream.Close();
			return address - startAddress;
		}

		public int Save(UInt16 startAddress, UInt16 endAddress, string path)
		{
			FileStream fileStream = File.Create(path);
			UInt16 address = startAddress;
			while (address <= endAddress) {
				fileStream.WriteByte(pagedMemory.ReadByte(address++));
			}
			fileStream.Close();
			return address - startAddress;
		}

		#endregion
	}
}