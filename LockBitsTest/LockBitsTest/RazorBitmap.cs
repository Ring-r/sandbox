using System.Runtime.InteropServices;
using System.Drawing;
using System;

namespace LockBitsTest
{
    /// <summary>
    /// Bitmap without using Graphics.DrawImage().
    /// </summary>
    /// <remarks>For Windows only. Some part get from <a href="http://razorgdipainter.codeplex.com/">Mokrov Ivan</a>.</remarks>
    public class RazorBitmap : ArrayBitmap, IDisposable
    {
        [DllImport("gdi32")]
        private extern static int SetDIBitsToDevice(HandleRef hDC, int xDest, int yDest, int dwWidth, int dwHeight, int XSrc, int YSrc, int uStartScan, int cScanLines, ref int lpvBits, ref BITMAPINFO lpbmi, uint fuColorUse);

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public int bihSize;
            public int bihWidth;
            public int bihHeight;
            public short bihPlanes;
            public short bihBitCount;
            public int bihCompression;
            public int bihSizeImage;
            public double bihXPelsPerMeter;
            public double bihClrUsed;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFO
        {
            public BITMAPINFOHEADER biHeader;
            public int biColors;
        }

        private BITMAPINFO bitmapInfo = new BITMAPINFO
        {
            biHeader =
            {
                bihBitCount = 32,
                bihPlanes = 1,
                bihSize = 40,
                bihWidth = 1,
                bihHeight = 1,
                bihSizeImage = 2
            }
        };

        private readonly Graphics graphics;
        private readonly IntPtr hdc;
        private readonly HandleRef handleRef;

        public Size Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = new Size(Math.Max(1, value.Width), Math.Max(1, value.Height));
                this.array = new int[this.size.Width * this.size.Height];
                this.bitmapInfo.biHeader.bihWidth = this.size.Width;
                this.bitmapInfo.biHeader.bihHeight = this.size.Height;
                this.bitmapInfo.biHeader.bihSizeImage = this.size.Width * this.size.Height;
            }
        }

        public RazorBitmap(Graphics graphics)
        {
            this.graphics = graphics;
            this.hdc = this.graphics.GetHdc();
            this.handleRef = new HandleRef(graphics, hdc);
        }

        public void Draw()
        {
            SetDIBitsToDevice(this.handleRef, 0, 0, this.size.Width, this.size.Height, 0, 0, 0, this.size.Height, ref this.array[0], ref this.bitmapInfo, 0);
        }

        public void Dispose()
        {
            this.graphics.ReleaseHdc(this.hdc);
            this.graphics.Dispose();
        }
    }
}
