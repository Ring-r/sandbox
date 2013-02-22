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
    public class FastBitmap : ArrayBitmap
    {
        private Bitmap bitmap;

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
                this.bitmap = new Bitmap(this.size.Width, this.size.Height);
            }
        }

        public Bitmap Image
        {
            get
            {
                return this.bitmap;
            }
            set
            {
                if (this.size.Width != value.Width || this.size.Height != value.Height)
                {
                    this.size = new Size(value.Width, value.Height);
                    this.array = new int[value.Width * value.Height];
                }
                BitmapData imageData = this.bitmap.LockBits(new Rectangle(0, 0, value.Width, value.Height), ImageLockMode.ReadWrite, value.PixelFormat);
                Marshal.Copy(imageData.Scan0, this.array, 0, this.array.Length);
                value.UnlockBits(imageData);
                this.bitmap = value;
            }
        }

        public FastBitmap()
        {
            this.bitmap = new Bitmap(1, 1);
        }

        public void RefreshImage()
        {
            BitmapData imageData = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), ImageLockMode.ReadWrite, this.bitmap.PixelFormat);
            Marshal.Copy(this.array, 0, imageData.Scan0, this.array.Length);
            this.bitmap.UnlockBits(imageData);
        }
    }
}
