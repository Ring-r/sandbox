using System.Collections.Generic;
using System.Drawing;
using Entities;
using System;

namespace DotWayTest
{
    class MilkyMan : EntityCircle
    {
        public readonly List<PointF> Dots = new List<PointF>(); //static
        public readonly List<int> dotsStack = new List<int>();
        public int index = -1;

        public void onManagedDraw(Graphics graphics)
        {
            graphics.FillEllipse(Brushes.Red, this.X, this.Y, 2 * this.Radius, 2 * this.Radius);
        }

        public void onManagedUpdate(float pSecondsElapsed)
        {
            if (this.index < 0)
            {
                this.index = 0;
                this.CenterX = Dots[this.index].X;
                this.CenterY = Dots[this.index].Y;
                this.Angle = Math.Atan2(Dots[this.index + 1].Y - Dots[this.index].Y, Dots[this.index + 1].X - Dots[this.index].X);
            }
            else if (index < Dots.Count - 1)
            {
                this.CenterX += this.Speed * pSecondsElapsed * this.DX;
                this.CenterY += this.Speed * pSecondsElapsed * this.DY;

                float x, y;

                x = Dots[this.index + 1].X - Dots[this.index].X;
                y = Dots[this.index + 1].Y - Dots[this.index].Y;
                double p2p1 = Math.Sqrt(x * x + y * y);

                x = this.CenterX - Dots[this.index].X;
                y = this.CenterY - Dots[this.index].Y;
                double pp1 = Math.Sqrt(x * x + y * y);

                while (pp1 > p2p1 && this.index < Options.DotsCount - 1)
                {
                    double d = pp1 - p2p1;
                    this.index++;
                    this.CenterX = Dots[this.index].X;
                    this.CenterY = Dots[this.index].Y;
                    if (this.index < Options.DotsCount - 1)
                    {
                        this.Angle = Math.Atan2(Dots[this.index + 1].Y - Dots[this.index].Y, Dots[this.index + 1].X - Dots[this.index].X);

                        this.CenterX += (float)d * this.DX;
                        this.CenterY += (float)d * this.DY;

                        x = Dots[this.index + 1].X - Dots[this.index].X;
                        y = Dots[this.index + 1].Y - Dots[this.index].Y;
                        p2p1 = Math.Sqrt(x * x + y * y);

                        x = this.CenterX - Dots[this.index].X;
                        y = this.CenterY - Dots[this.index].Y;
                        pp1 = Math.Sqrt(x * x + y * y);
                    }
                }
            }
        }

        public float Speed { get; set; }
        public double Angle { get; set; }
        public float DX
        {
            get
            {
                return (float)Math.Cos(this.Angle);
            }
        }
        public float DY
        {
            get
            {
                return (float)Math.Sin(this.Angle);
            }
        }
    }
}
