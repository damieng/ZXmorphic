using Morphic.Core.Bus;
using Morphic.Core.Memory;
using System;
using System.IO;

namespace Spectrum.Memory
{
	public class Memory48K : IBus16Bit
	{
		public PagedMemory PagedMemory;

		public Memory48K()
		{
			Reset();
		}

		public void Reset()
		{
			PagedMemory = new PagedMemory(4, 16384, 4);
			PagedMemory.AllPages[0].ReadOnly = true;
		}

		public Byte ReadByte(UInt16 address)
		{
			return PagedMemory.ReadByte(address);
		}

		public void WriteByte(UInt16 address, Byte data)
		{
			PagedMemory.WriteByte(address, data, false);
		}

		public UInt16 ReadWord(UInt16 address)
		{
			return PagedMemory.ReadWord(address);
		}

		public void WriteWord(UInt16 address, UInt16 data)
		{
			PagedMemory.WriteWord(address, data, false);
		}

		public int Load(UInt16 startAddress, string path)
		{
		    using (var fileStream = File.OpenRead(path))
		    {
		        var address = startAddress;
		        var nextByte = fileStream.ReadByte();
		        while (nextByte != -1)
		        {
		            PagedMemory.WriteByte(address++, (Byte) nextByte, true);
		            nextByte = fileStream.ReadByte();
		        }
		        return address - startAddress;
		    }
		}

		public int Save(UInt16 startAddress, UInt16 endAddress, string path)
		{
		    using (var fileStream = File.Create(path))
		    {
		        var address = startAddress;
		        while (address <= endAddress)
		            fileStream.WriteByte(PagedMemory.ReadByte(address++));
		        return address - startAddress;
		    }
		}
	}
}