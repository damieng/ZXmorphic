using System;

namespace Morphic.Core.Memory
{
	public class MemoryPage
	{
		private readonly Byte[] bytes;
		public Boolean ReadOnly;

		public MemoryPage(UInt32 size, Boolean readOnly)
		{
			bytes = new Byte[size];
			ReadOnly = readOnly;
		}

		public Byte this[Int32 address]
		{
			get { return bytes[address]; }
			set { bytes[address] = value; }
		}

		public MemoryPage[] Create(UInt32 pageCount, UInt32 pageSize)
		{
			var pages = new MemoryPage[pageCount];
			for(var idx = 0; idx < pageCount; idx++)
				pages[idx] = new MemoryPage(pageSize, false);
			return pages;
		}
	}
}