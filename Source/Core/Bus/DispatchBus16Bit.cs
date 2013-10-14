using System;
using System.Collections.Generic;

namespace Morphic.Core.Bus
{
	// TODO
	// 1. Remove interfaces and allow for delegates directly
	// 2. Allow multiple listeners for an address

	public interface IReadBus16Bit
	{
		Byte ReadByte(UInt16 address);
		UInt16 ReadWord(UInt16 address);
	}

	public interface IWriterBus16Bit
	{
		void Write(UInt16 address, Byte data);
		void Write(UInt16 address, UInt16 data);
	}

	public class DispatchBus16Bit : IBus16Bit
	{
		private readonly Dictionary<UInt16, IReadBus16Bit> readers = new Dictionary<UInt16, IReadBus16Bit>();
		private readonly Dictionary<UInt16, IWriterBus16Bit> writers = new Dictionary<UInt16, IWriterBus16Bit>();

		public Byte ReadByte(UInt16 address)
		{
			var reader = readers[address];
			return (reader == null) ? Byte.MinValue : reader.ReadByte(address);
		}

		public void WriteByte(UInt16 address, Byte data)
		{
			var writer = writers[address];
			if (writer != null)
				writer.Write(address, data);
		}

		public UInt16 ReadWord(UInt16 address)
		{
			var reader = readers[address];
			return (reader == null) ? UInt16.MinValue : reader.ReadWord(address);
		}

		public void WriteWord(UInt16 address, UInt16 data)
		{
			var writer = writers[address];
			if (writer != null)
				writer.Write(address, data);
		}
	}
}