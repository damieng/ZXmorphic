using System;

namespace Morphic.Core.Primitive
{
	public struct LEWord
	{
		#region Instance variables

		public Byte Low, High;

		#endregion

		#region Construction

		public LEWord(Byte low, Byte high)
		{
			this.Low = low;
			this.High = high;
		}

		public LEWord(UInt16 word)
		{
			this.High = (Byte) (word / 256);
			this.Low = (Byte) (word % 256);
		}

		#endregion

		#region Methods

		public void IncLow()
		{
			if (Low == 255)
				Low = 0;
			else
				Low++;
		}

		public void IncHigh()
		{
			if (High == 255)
				High = 0;
			else {
				High++;
			}
		}

		public void Inc()
		{
			if (Low != 255)
				Low++;
			else {
				Low = 0;
				if (High != 255)
					High++;
				else
					High = 0;
			}
		}

		public void DecLow()
		{
			if (Low == 0)
				Low = 255;
			else
				Low--;
		}

		public void DecHigh()
		{
			if (High == 0)
				High = 255;
			else
				High--;
		}

		public void Dec()
		{
			if (Low != 0)
				Low--;
			else {
				Low = 255;
				if (High != 0)
					High--;
				else
					High = 255;
			}
		}

		public UInt16 ToUInt16()
		{
			return (UInt16)((High * 256) + Low);
		}

		public override String ToString()
		{
			return String.Format("{0:X4}", ToUInt16());
		}

		#endregion

		#region Static methods

		public static implicit operator UInt16(LEWord value)
		{
			return value.ToUInt16();
		}

		public static implicit operator LEWord(UInt16 value)
		{
			return new LEWord(value);
		}

		#endregion
	}
}