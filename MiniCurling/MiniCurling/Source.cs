using System;

namespace MiniCurling
{
    public class Source : Circle
    {
        private const float a = -1.0f;
        private float v;
        private float vx, vy;

        public Source(float radius)
            : base(radius)
        {
        }

        public void Push(float vx, float vy)
        {
            this.v = (float)Math.Sqrt(vx * vx + vy * vy);
            if (this.v > 0.0f)
            {
                this.vx = vx / this.v;
                this.vy = vy / this.v;
            }
        }

        public void Move(float maxX, float maxY)
        {
            this.px += this.v * this.vx;
            this.py += this.v * this.vy;

            this.v = this.v + a > 0 ? this.v + a : 0.0f;

            bool isRun;
            do
            {
                isRun = false;
                if (this.px - this.radius < 0)
                {
                    this.px = 2 * this.radius - this.px;
                    this.vx = +Math.Abs(this.vx);
                    isRun = true;
                }
                if (this.px + this.radius > maxX)
                {
                    this.px = 2 * (maxX - this.radius) - this.px;
                    this.vx = -Math.Abs(this.vx);
                    isRun = true;
                }

                if (this.py - this.radius < 0)
                {
                    this.py = 2 * this.radius - this.py;
                    this.vy = +Math.Abs(this.vy);
                    isRun = true;
                }
                if (this.py + this.radius > maxY)
                {
                    this.py = 2 * (maxY - this.radius) - this.py;
                    this.vy = -Math.Abs(this.vy);
                    isRun = true;
                }
            } while (isRun);
        }

        public bool IsMove()
        {
            return this.v != 0.0f;
        }
    }
}
