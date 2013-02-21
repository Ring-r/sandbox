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
    class FastBitmap
    {
        private const int BitsPerPixel = 4;

        private Size _imageSize;
        private byte[] _store;


        public FastBitmap()
        {
            _imageSize = new Size(1, 1);
            _store = new byte[BitsPerPixel];
        }

        public FastBitmap(Bitmap original)
        {
            _imageSize = new Size(original.Width, original.Height);
            _store = new byte[BitsPerPixel * _imageSize.Width * _imageSize.Height];
            BitmapData imageData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadWrite, original.PixelFormat);
            Marshal.Copy(imageData.Scan0, _store, 0, imageData.Stride * imageData.Height);
            original.UnlockBits(imageData);
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
    }
}
