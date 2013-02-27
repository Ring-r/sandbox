using System.Collections.Generic;
using System.Drawing;
using Entities;
using System;

namespace DotWayTest
{
    class MilkyMan : EntityCircle, IEntity
    {
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

        public Brush Brush = Brushes.Red;

        public void onManagedDraw(Graphics graphics)
        {
            if (this.IsDrawWay)
            {
                this.DrawWay(graphics);
            }
            graphics.FillEllipse(this.Brush, this.X, this.Y, 2 * this.Radius, 2 * this.Radius);
        }

        public void onManagedUpdate(float pSecondsElapsed)
        {
            if (this.dotsIndex < this.dotsStack.Count - 1)
            {
                int currIndex = this.dotsStack[this.dotsIndex];
                int nextIndex = this.dotsStack[this.dotsIndex + 1];

                this.CenterX += this.Speed * pSecondsElapsed * this.DX;
                this.CenterY += this.Speed * pSecondsElapsed * this.DY;

                float x, y;

                x = this.map.Dots[nextIndex].X - this.map.Dots[currIndex].X;
                y = this.map.Dots[nextIndex].Y - this.map.Dots[currIndex].Y;
                double p2p1 = Math.Sqrt(x * x + y * y);

                x = this.CenterX - this.map.Dots[currIndex].X;
                y = this.CenterY - this.map.Dots[currIndex].Y;
                double pp1 = Math.Sqrt(x * x + y * y);

                while (pp1 > p2p1 && this.dotsIndex < this.dotsStack.Count - 1)
                {
                    double d = pp1 - p2p1;
                    this.dotsIndex++;
                    currIndex = this.dotsStack[this.dotsIndex];

                    this.CenterX = this.map.Dots[currIndex].X;
                    this.CenterY = this.map.Dots[currIndex].Y;
                    if (this.dotsIndex < this.dotsStack.Count - 1)
                    {
                        nextIndex = this.dotsStack[this.dotsIndex + 1];
                        this.Angle = Math.Atan2(this.map.Dots[nextIndex].Y - this.map.Dots[currIndex].Y, this.map.Dots[nextIndex].X - this.map.Dots[currIndex].X);

                        this.CenterX += (float)d * this.DX;
                        this.CenterY += (float)d * this.DY;

                        x = this.map.Dots[nextIndex].X - this.map.Dots[currIndex].X;
                        y = this.map.Dots[nextIndex].Y - this.map.Dots[currIndex].Y;
                        p2p1 = Math.Sqrt(x * x + y * y);

                        x = this.CenterX - this.map.Dots[currIndex].X;
                        y = this.CenterY - this.map.Dots[currIndex].Y;
                        pp1 = Math.Sqrt(x * x + y * y);
                    }
                }
            }
        }


        public bool IsDrawWay = false;
        private void DrawWay(Graphics graphics)
        {
            for (int i = 0; i < this.dotsStack.Count - 1; ++i)
            {
                // TODO: Correct view.
                PointF p1 = this.map.Dots[this.dotsStack[i]];
                PointF p2 = this.map.Dots[this.dotsStack[i + 1]];
                graphics.DrawLine(Pens.Yellow, p1, p2);
            }
        }


        public Map map = null;
        public int dotsIndex = 0;
        public readonly List<int> dotsStack = new List<int>();
        private readonly List<int> dotsChecker = new List<int>();

        public void Init()
        {
            if (this.map.Dots != null)
            {
                this.dotsIndex = 0;
                this.dotsStack.Clear();
                this.dotsStack.Add(0);
                this.dotsChecker.Clear();
                for (int i = this.map.Dots.Length - 1; i >= 0; --i)
                {
                    this.dotsChecker.Add(0);
                }
                this.dotsChecker[0] = 1;
            }
        }

        public void AddDotNear(Point point)
        {
            int indexMin = 0;
            double distanceMin = float.PositiveInfinity;
            for (int i = 0; i < this.map.Dots.Length; ++i)
            {
                PointF dot = this.map.Dots[i];
                float x = point.X - dot.X;
                float y = point.Y - dot.Y;
                double d = x * x + y * y;
                if (distanceMin > d)
                {
                    distanceMin = d;
                    indexMin = i;
                }
            }
            if (distanceMin < Options.DotsRadius * Options.DotsRadius)
            {
                this.dotsIndex = indexMin;
                this.dotsStack.Add(this.dotsIndex);
                this.dotsChecker[this.dotsIndex]++;
            }
        }

        public void FinishDotsStack() // TODO: Write FinishDotsStackAnother(), FinishDotsStackRandom().
        {
            for (int i = 0; i < this.dotsStack.Count; ++i)
            {
                this.dotsChecker[this.dotsStack[i]] = 1;
            }
            for (int i = 0; i < this.map.Dots.Length - 2; ++i)
            {
                int k = 0;
                double md = float.PositiveInfinity;
                for (int j = 1; j < this.map.Dots.Length - 1; ++j)
                {
                    if (this.dotsChecker[j] == 0)
                    {
                        float x = this.map.Dots[this.dotsStack[this.dotsStack.Count - 1]].X - this.map.Dots[j].X;
                        float y = this.map.Dots[this.dotsStack[this.dotsStack.Count - 1]].Y - this.map.Dots[j].Y;
                        double d = Math.Sqrt(x * x + y * y);
                        if (md > d)
                        {
                            md = d;
                            k = j;
                        }
                    }
                }
                this.dotsStack.Add(k);
                this.dotsChecker[k]++;
            }
            this.dotsStack.Add(this.map.Dots.Length - 1);
        }

        public void FinishDotsStackRandom()
        {
            for (int i = 0; i < this.dotsStack.Count; ++i)
            {
                this.dotsChecker[this.dotsStack[i]] = 1;
            }

            int[] randArray = new int[this.map.Dots.Length];
            for (int i = 0; i < this.map.Dots.Length; ++i)
            {
                randArray[i] = i;
            }
            for (int i = 0; i < this.map.Dots.Length; ++i)
            {
                int j = Options.random.Next(1, this.map.Dots.Length - 1);
                int k = Options.random.Next(1, this.map.Dots.Length - 1);
                int r = randArray[j];
                randArray[j] = randArray[k];
                randArray[k] = r;
            }
            for (int i = 0; i < this.map.Dots.Length; ++i)
            {
                if (this.dotsChecker[randArray[i]] == 0)
                {
                    this.dotsStack.Add(randArray[i]);
                    this.dotsChecker[randArray[i]]++;
                }
            }

            if (this.dotsStack[this.dotsStack.Count - 1] != this.map.Dots.Length - 1)
            {
                this.dotsStack.Add(Options.DotsCount - 1);
            }
        }


        public bool IsFull
        {
            get
            {
                bool isFinish = this.dotsStack[this.dotsStack.Count - 1] == this.map.Dots.Length - 1;
                for (int i = this.dotsChecker.Count - 1; i >= 0 && isFinish; --i)
                {
                    isFinish = isFinish && this.dotsChecker[i] > 0;
                }
                return isFinish;
            }
        }
    }
}
