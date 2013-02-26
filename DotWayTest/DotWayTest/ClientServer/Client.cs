using System.Drawing;
using System.Collections.Generic;
using System;

namespace DotWayTest
{
    class Client
    {
        private int currentDotsIndex = 0;
        private List<int> dotsStack = new List<int>(); // TODO: Replace by array.
        private int[] dotsChecker = null;

        public void DotsInit()
        {
            this.dotsStack.Clear();
            this.dotsStack.Add(0);
            if (this.dotsChecker.Length <= Options.Dots.Length)
            {
                Array.Clear(this.dotsChecker, 0, this.dotsChecker.Length);
            }
            else
            {
                this.dotsChecker = new int[Options.Dots.Length];
            }

            //Options.CurrentDistance = 0;
            this.currentDotsIndex = 0;
            this.dotsStack.Clear();
            this.dotsStack.Add(this.currentDotsIndex);
            this.dotsChecker = new int[Options.DotsCount];
            this.dotsChecker[this.currentDotsIndex]++;
            Options.IsFinish = false;
        }

        private void FinishDotsStack()
        {
            for (int i = 0; i < this.dotsStack.Count; ++i)
            {
                this.dotsChecker[this.dotsStack[i]] = 1;
            }
            for (int i = 0; i < Options.DotsCount - 2; ++i)
            {
                int k = 0;
                double md = float.PositiveInfinity;
                for (int j = 1; j < Options.DotsCount - 1; ++j)
                {
                    if (this.dotsChecker[j] == 0)
                    {
                        float x = Options.Dots[this.dotsStack[this.dotsStack.Count - 1]].X - Options.Dots[j].X;
                        float y = Options.Dots[this.dotsStack[this.dotsStack.Count - 1]].Y - Options.Dots[j].Y;
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
            this.dotsStack.Add(Options.DotsCount - 1);
        }
    }
}
