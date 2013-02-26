using System.Drawing;

namespace DotWayTest
{
    class Server
    {
        private int currentDotsIndex = 0;

        public void DotsInit()
        {
            // TODO: if(dots.Length < Options.DotsCount) ...
            Options.Dots = new Point[Options.DotsCount];
            for (int i = 0; i < Options.DotsCount; ++i)
            {
                Options.Dots[i] = new Point(
                    Options.random.Next((int)(Options.MapWidth * Options.BorderSize), (int)(Options.MapWidth * (1 - Options.BorderSize))),
                    Options.random.Next((int)(Options.MapHeight * Options.BorderSize), (int)(Options.MapHeight * (1 - Options.BorderSize))));
            }
            this.currentDotsIndex = 0;
            Options.IsFinish = false;
        }
    }
}
