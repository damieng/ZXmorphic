using System;
using System.Drawing;

using Morphic.Core.Memory;

namespace Spectrum.Custom
{
	public class SpectrumDisplay
	{
		private static UInt16[] lookupY = new UInt16[pixelHeight];

		private const int attributeWidth = 32;
		private const int attributeHeight = 24;
		private const int pixelWidth = 8 * attributeWidth;
		private const int pixelHeight = 8 * attributeHeight;
		private const int attributeOffset = pixelWidth * attributeHeight;

		private MemoryPage memoryPage;
		private Color[] pallette;
		private Bitmap bitmap;

		public SpectrumDisplay(MemoryPage memoryPage)
		{
			this.memoryPage = memoryPage;
			bitmap = new Bitmap(pixelWidth, pixelHeight);
			pallette = new Color[] {
				Color.Black, Color.DarkBlue, Color.DarkRed, Color.DarkMagenta, Color.Green, Color.DarkCyan, Color.Olive, Color.LightGray,
				Color.Black, Color.Blue, Color.Red, Color.Magenta, Color.LimeGreen, Color.Cyan, Color.Yellow, Color.White };
		}

		public Bitmap GetBitmap()
		{
			for(int ay = 0; ay < attributeHeight; ay++)
				for(int ax = 0; ax < attributeWidth; ax++) {
					Byte attribute = memoryPage[ay * attributeWidth + attributeOffset + ax];
					Byte bright = (Byte)(((UInt16)attribute & 64) >> 3);
					Color foreColor = pallette[(attribute & 7) | bright ];
					Color backColor = pallette[((attribute & 56) >> 3) | bright];
					for(int py = 0; py < 8; py++) {
						int y = ay * 8 + py;
						Byte pixels = memoryPage[lookupY[y] + ax];
						for (int px = 0; px < 8; px++) {
							int a = 128 >> px;
							int x = ax * 8 + px;
							bitmap.SetPixel(x, y, (pixels & a) != 0 ? foreColor : backColor);
						}
					}
				}

			return bitmap;
		}

		static SpectrumDisplay()
		{
			UInt16 pos = 0;
			for(int third = 0; third < 3 ; third++)
				for (int line = 0; line < 8; line++)
					for (int y = 0; y < 63; y += 8) {
						lookupY[y + line + (third * 64)] = pos;
						pos += 32;
					}
		}
	}
}
