using System;
using System.Drawing;
using Morphic.Core.Memory;

namespace Spectrum.Custom
{
    public class SpectrumDisplay
    {
        private const int AttributeWidth = 32;
        private const int AttributeHeight = 24;
        private const int PixelWidth = 8 * AttributeWidth;
        private const int PixelHeight = 8 * AttributeHeight;
        private const int AttributeOffset = PixelWidth * AttributeHeight;

        private static readonly UInt16[] lookupY = new UInt16[PixelHeight];
        private readonly Bitmap bitmap;

        private readonly MemoryPage memoryPage;
        private readonly Color[] palette;

        static SpectrumDisplay()
        {
            UInt16 pos = 0;
            for (var third = 0; third < 3; third++)
                for (var line = 0; line < 8; line++)
                    for (var y = 0; y < 63; y += 8)
                    {
                        lookupY[y + line + (third * 64)] = pos;
                        pos += 32;
                    }
        }

        public SpectrumDisplay(MemoryPage memoryPage)
        {
            this.memoryPage = memoryPage;
            bitmap = new Bitmap(PixelWidth, PixelHeight);
            palette = new[]
            {
                Color.Black, Color.DarkBlue, Color.DarkRed, Color.DarkMagenta, Color.Green, Color.DarkCyan, Color.Olive,
                Color.LightGray,
                Color.Black, Color.Blue, Color.Red, Color.Magenta, Color.LimeGreen, Color.Cyan, Color.Yellow,
                Color.White
            };
        }

        public Bitmap GetBitmap()
        {
            for (var ay = 0; ay < AttributeHeight; ay++)
                for (var ax = 0; ax < AttributeWidth; ax++)
                {
                    var attribute = memoryPage[ay * AttributeWidth + AttributeOffset + ax];
                    var bright = (Byte)((attribute & 64) >> 3);
                    var foreColor = palette[(attribute & 7) | bright];
                    var backColor = palette[((attribute & 56) >> 3) | bright];
                    for (var py = 0; py < 8; py++)
                    {
                        var y = ay * 8 + py;
                        var pixels = memoryPage[lookupY[y] + ax];
                        for (var px = 0; px < 8; px++)
                        {
                            var a = 128 >> px;
                            var x = ax * 8 + px;
                            bitmap.SetPixel(x, y, (pixels & a) != 0 ? foreColor : backColor);
                        }
                    }
                }

            return bitmap;
        }
    }
}