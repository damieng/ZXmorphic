using System;
using System.Collections.Generic;

using Morphic.Core.Bus;

namespace Spectrum.Custom
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
		public Dictionary<UInt16, IReadBus16Bit> readers = new Dictionary<UInt16, IReadBus16Bit>();
		public Dictionary<UInt16, IWriterBus16Bit> writers = new Dictionary<UInt16, IWriterBus16Bit>();

		#region I16BitBus IO interface

		public Byte ReadByte(UInt16 address)
		{
			IReadBus16Bit reader = readers[address];
			return (reader == null) ? Byte.MinValue : reader.ReadByte(address);
		}

		public void WriteByte(UInt16 address, Byte data)
		{
			IWriterBus16Bit writer = writers[address];
			if (writer != null)
				writer.Write(address, data);
		}

		public UInt16 ReadWord(UInt16 address)
		{
			IReadBus16Bit reader = readers[address];
			return (reader == null) ? UInt16.MinValue : reader.ReadWord(address);
		}

		public void WriteWord(UInt16 address, UInt16 data)
		{
			IWriterBus16Bit writer = writers[address];
			if (writer != null)
				writer.Write(address, data);
		}

		#endregion
	}
}