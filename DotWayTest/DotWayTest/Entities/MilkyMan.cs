using System.Collections.Generic;
using System.Drawing;
using Entities;
using System;

namespace DotWayTest
{
    class MilkyMan : EntityCircle, IEntity
    {
        public readonly List<PointF> Dots = new List<PointF>();
        public readonly List<int> dotsStack = new List<int>();
        public int dotsIndex = -1;

        private bool isDrawWay = false;
        private void DrawWay(Graphics graphics)
        {
            for (int i = 0; i < this.dotsStack.Count - 1; ++i)
            {
                // TODO: Correct view.
                PointF p1 = Options.Dots[this.dotsStack[i]];
                PointF p2 = Options.Dots[this.dotsStack[i + 1]];
                graphics.DrawLine(Pens.Yellow, p1, p2);
            }
        }

        public void onManagedDraw(Graphics graphics)
        {
            if (this.isDrawWay)
            {
                this.DrawWay(graphics);
            }
            graphics.FillEllipse(Brushes.Red, this.X, this.Y, 2 * this.Radius, 2 * this.Radius);
        }

        public void onManagedUpdate(float pSecondsElapsed)
        {
            if (this.dotsIndex < 0)
            {
                this.dotsIndex = 0;
                this.CenterX = Dots[this.dotsIndex].X;
                this.CenterY = Dots[this.dotsIndex].Y;
                this.Angle = Math.Atan2(Dots[this.dotsIndex + 1].Y - Dots[this.dotsIndex].Y, Dots[this.dotsIndex + 1].X - Dots[this.dotsIndex].X);
            }
            else if (dotsIndex < Dots.Count - 1)
            {
                this.CenterX += this.Speed * pSecondsElapsed * this.DX;
                this.CenterY += this.Speed * pSecondsElapsed * this.DY;

                float x, y;

                x = Dots[this.dotsIndex + 1].X - Dots[this.dotsIndex].X;
                y = Dots[this.dotsIndex + 1].Y - Dots[this.dotsIndex].Y;
                double p2p1 = Math.Sqrt(x * x + y * y);

                x = this.CenterX - Dots[this.dotsIndex].X;
                y = this.CenterY - Dots[this.dotsIndex].Y;
                double pp1 = Math.Sqrt(x * x + y * y);

                while (pp1 > p2p1 && this.dotsIndex < Options.DotsCount - 1)
                {
                    double d = pp1 - p2p1;
                    this.dotsIndex++;
                    this.CenterX = Dots[this.dotsIndex].X;
                    this.CenterY = Dots[this.dotsIndex].Y;
                    if (this.dotsIndex < Options.DotsCount - 1)
                    {
                        this.Angle = Math.Atan2(Dots[this.dotsIndex + 1].Y - Dots[this.dotsIndex].Y, Dots[this.dotsIndex + 1].X - Dots[this.dotsIndex].X);

                        this.CenterX += (float)d * this.DX;
                        this.CenterY += (float)d * this.DY;

                        x = Dots[this.dotsIndex + 1].X - Dots[this.dotsIndex].X;
                        y = Dots[this.dotsIndex + 1].Y - Dots[this.dotsIndex].Y;
                        p2p1 = Math.Sqrt(x * x + y * y);

                        x = this.CenterX - Dots[this.dotsIndex].X;
                        y = this.CenterY - Dots[this.dotsIndex].Y;
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
