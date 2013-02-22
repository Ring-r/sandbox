using System;
using System.Drawing;

namespace LockBitsTest
{
    public abstract class ArrayBitmap
    {
        protected Size size;
        protected int[] array;

        public ArrayBitmap()
        {
            this.size = new Size(1, 1);
            this.array = new int[1];
        }

        public int GetPixel(int x, int y)
        {
            int pointBase = y * this.size.Width + x;
            if (pointBase < this.array.Length)
            {
                return this.array[pointBase];
            }
            return 0;
        }

        public void SetPixel(int x, int y, int color)
        {
            int pointBase = y * this.size.Width + x;
            if (pointBase < this.array.Length)
            {
                this.array[pointBase] = color;
            }
        }

        public void SetPixel(int x, int y, Color color)
        {
            int pointBase = y * this.size.Width + x;
            if (pointBase < this.array.Length)
            {
                this.array[pointBase] = color.ToArgb();
            }
        }

        public void SetPixel(int x, int y, byte a, byte r, byte g, byte b)
        {
            int pointBase = y * this.size.Width + x;
            if (pointBase < this.array.Length)
            {
                this.array[pointBase] = b << 6 + g << 4 + b << 2 + a;
            }
        }

        public void Clear()
        {
            Array.Clear(this.array, 0, this.array.Length);
        }
    }
}
