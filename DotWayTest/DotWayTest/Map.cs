using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotWayTest
{
    class Map
    {
        public Size Size = new Size(300, 300);
        public const float BorderFactor = 0.1f;

        public Point[] Dots = null;
        public MilkyMan milkyMan = null;
        public readonly List<MilkyMan> milkyManList = new List<MilkyMan>();

        public void DotsInit()
        {
            // TODO: if(dots.Length < Options.DotsCount) ...
            this.Dots = new Point[Options.DotsCount];
            for (int i = 0; i < Options.DotsCount; ++i)
            {
                this.Dots[i] = new Point(
                    Options.random.Next((int)(this.Size.Width * BorderFactor), (int)(this.Size.Width * (1 - BorderFactor))),
                    Options.random.Next((int)(this.Size.Height * BorderFactor), (int)(this.Size.Height * (1 - BorderFactor))));
            }
        }

        public void MilkyManOnStart()
        {
            foreach (MilkyMan milkyMan in this.milkyManList)
            {
                milkyMan.dotsIndex = 0;
                milkyMan.CenterX = this.Dots[milkyMan.dotsStack[0]].X;
                milkyMan.CenterY = this.Dots[milkyMan.dotsStack[0]].Y;
                milkyMan.Angle = Math.Atan2(this.Dots[milkyMan.dotsStack[1]].Y - this.Dots[milkyMan.dotsStack[0]].Y, this.Dots[milkyMan.dotsStack[1]].X - this.Dots[milkyMan.dotsStack[0]].X);
            }
        }

        public void onManagedDraw(Graphics graphics)
        {
            if (this.Dots != null)
            {
                for (int i = 0; i < this.Dots.Length; ++i)
                {
                    PointF dot = this.Dots[i];
                    graphics.FillEllipse(Brushes.Silver, dot.X - Options.DotsRadius, dot.Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);
                }
                graphics.FillEllipse(Brushes.Green, this.Dots[0].X - Options.DotsRadius, this.Dots[0].Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);
                graphics.FillEllipse(Brushes.Red, this.Dots[Options.DotsCount - 1].X - Options.DotsRadius, this.Dots[Options.DotsCount - 1].Y - Options.DotsRadius, 2 * Options.DotsRadius, 2 * Options.DotsRadius);

                foreach (MilkyMan milkyMan in this.milkyManList)
                {
                    milkyMan.onManagedDraw(graphics);
                }
            }
        }

        public void onManagedUpdate(float secondsElapsed)
        {
            foreach (MilkyMan milkyMan in this.milkyManList)
            {
                milkyMan.onManagedUpdate(secondsElapsed);
            }
        }
    }
}
