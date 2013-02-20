using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LockBitsTest
{
    /// <summary>
    /// Bitmap with using LockBits.
    /// </summary>
    /// <remarks>Some part get from <a href="http://www.cyberforum.ru/post1858797.html">Killster</a>.</remarks>
    /// <remarks>Some part get from <a href="http://razorgdipainter.codeplex.com/">Mokrov Ivan</a>.</remarks>
    class FastBitmap
    {
        private const int BitsPerPixel = 4;

        private Size _imageSize;
        private byte[] _store;

        BITMAPINFO _BI = new BITMAPINFO
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

        public FastBitmap()
        {
            _imageSize = new Size(1, 1); // 1 пиксел.
            _store = new byte[BitsPerPixel];
        }

        public FastBitmap(Bitmap original)
        {
            _imageSize = new Size(original.Width, original.Height);
            _store = new byte[BitsPerPixel * _imageSize.Width * _imageSize.Height];
            BitmapData imageData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadWrite, original.PixelFormat);
            Marshal.Copy(imageData.Scan0, _store, 0, imageData.Stride * imageData.Height);
            original.UnlockBits(imageData);
            // TODO: Check.
            _BI.biHeader.bihWidth = _imageSize.Width;
            _BI.biHeader.bihWidth = _imageSize.Height;
            _BI.biHeader.bihSizeImage = _imageSize.Width * _imageSize.Height << 2;
        }

        public FastBitmap(int width, int height)
        {
            _imageSize = new Size(width, height);
            _store = new byte[BitsPerPixel * width * height];
        }

        public Bitmap Image
        {
            get
            {
                Bitmap im = new Bitmap(_imageSize.Width, _imageSize.Height);
                BitmapData imageData = im.LockBits(new Rectangle(0, 0, im.Width, im.Height), ImageLockMode.ReadWrite, im.PixelFormat);
                Marshal.Copy(_store, 0, imageData.Scan0, _store.Length);
                im.UnlockBits(imageData);
                return im;
            }
            set
            {
                BitmapData imageData = value.LockBits(new Rectangle(0, 0, value.Width, value.Height), ImageLockMode.ReadWrite, value.PixelFormat);
                int imageByteCount = imageData.Stride * imageData.Height;
                _store = new byte[imageByteCount];
                _imageSize = new Size(value.Width, value.Height);
                Marshal.Copy(imageData.Scan0, _store, 0, imageByteCount);
                value.UnlockBits(imageData);
                // TODO: Check.
                _BI.biHeader.bihWidth = _imageSize.Width;
                _BI.biHeader.bihWidth = _imageSize.Height;
                _BI.biHeader.bihSizeImage = _imageSize.Width * _imageSize.Height << 2;
            }
        }

        public Color GetPixel(int x, int y)
        {
            int pointBase = y * _imageSize.Width * BitsPerPixel + x * BitsPerPixel;
            if (_store.Length < pointBase + 3) throw new ArgumentException();
            byte r = _store[pointBase + 2];
            byte g = _store[pointBase + 1];
            byte b = _store[pointBase];
            byte a = _store[pointBase + 3];
            return Color.FromArgb(a, r, g, b);
        }

        public void SetPixel(int x, int y, Color color)
        {
            int pointBase = y * _imageSize.Width * BitsPerPixel + x * BitsPerPixel;
            if (_store.Length < pointBase + 3) throw new ArgumentException();
            _store[pointBase] = color.B;
            _store[pointBase + 1] = color.G;
            _store[pointBase + 2] = color.R;
            _store[pointBase + 3] = color.A;
        }

        public void SetPixel(int x, int y, byte a, byte r, byte g, byte b)
        {
            int pointBase = y * _imageSize.Width * BitsPerPixel + x * BitsPerPixel;
            if (_store.Length < pointBase + 3) throw new ArgumentException();
            _store[pointBase] = b;
            _store[pointBase + 1] = g;
            _store[pointBase + 2] = r;
            _store[pointBase + 3] = a;
        }

        public void SetPixel(int x, int y, byte r, byte g, byte b)
        {
            int pointBase = y * _imageSize.Width * BitsPerPixel + x * BitsPerPixel;
            if (_store.Length < pointBase + 3) throw new ArgumentException();
            _store[pointBase] = b;
            _store[pointBase + 1] = g;
            _store[pointBase + 2] = r;
            _store[pointBase + 3] = 255;
        }

        public void Clear()
        {
            Array.Clear(_store, 0, _store.Length);
        }

        [DllImport("gdi32")]
        private extern static int SetDIBitsToDevice(HandleRef hDC, int xDest, int yDest, int dwWidth, int dwHeight, int XSrc, int YSrc, int uStartScan, int cScanLines, ref byte lpvBits, ref BITMAPINFO lpbmi, uint fuColorUse);

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

        public void Paint(HandleRef handleRef)
        {
            SetDIBitsToDevice(handleRef, 0, 0, _imageSize.Width, _imageSize.Height, 0, 0, 0, _imageSize.Height, ref _store[0], ref _BI, 0);
        }
    }
}
