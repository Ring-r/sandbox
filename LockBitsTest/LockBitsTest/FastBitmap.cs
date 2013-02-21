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
        private Size _imageSize;
        private int[] _store;

        public FastBitmap()
        {
            _imageSize = new Size(1, 1);
            _store = new int[1];
        }

        public FastBitmap(Bitmap original)
        {
            _imageSize = new Size(original.Width, original.Height);
            _store = new int[_imageSize.Width * _imageSize.Height];
            BitmapData imageData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadWrite, original.PixelFormat);
            Marshal.Copy(imageData.Scan0, _store, 0, _store.Length);
            original.UnlockBits(imageData);
        }

        public FastBitmap(int width, int height)
        {
            _imageSize = new Size(width, height);
            _store = new int[width * height];
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
                _store = new int[imageData.Width * imageData.Height];
                _imageSize = new Size(value.Width, value.Height);
                Marshal.Copy(imageData.Scan0, _store, 0, _store.Length);
                value.UnlockBits(imageData);
            }
        }

        public int GetPixel(int x, int y)
        {
            int pointBase = y * _imageSize.Width + x;
            if (pointBase < _store.Length)
            {
                return _store[pointBase];
            }
            return 0;
        }

        public void SetPixel(int x, int y, int color)
        {
            int pointBase = y * _imageSize.Width + x;
            if (pointBase < _store.Length)
            {
                _store[pointBase] = color;
            }
        }

        public void Clear()
        {
            Array.Clear(_store, 0, _store.Length);
        }
    }
}
