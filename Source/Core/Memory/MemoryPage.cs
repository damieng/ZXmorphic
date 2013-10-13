using System;

namespace Morphic.Core.Memory
{
	public class MemoryPage
	{
		#region Instance variables

		private Byte[] bytes;
		public Boolean ReadOnly;

		#endregion

		#region Construction

		public MemoryPage(UInt32 size, Boolean readOnly)
		{
			bytes = new Byte[size];
			this.ReadOnly = readOnly;
		}

		#endregion

		#region Properties

		public Byte this[Int32 address]
		{
			get { return bytes[address]; }
			set { bytes[address] = value; }
		}

		#endregion

		#region Static methods

		public MemoryPage[] Create(UInt32 pageCount, UInt32 pageSize)
		{
			MemoryPage[] pages = new MemoryPage[pageCount];
			for(int idx = 0; idx < pageCount; idx++)
				pages[idx] = new MemoryPage(pageSize, false);
			return pages;
		}

		#endregion
	}
}