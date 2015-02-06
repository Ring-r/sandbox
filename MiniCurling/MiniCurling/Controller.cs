using System;
using System.Drawing;

namespace MiniCurling
{
    public class Controller
    {
        private bool isRun = false;
        private Point point;
        private Point point_;

        public int GetVectorX()
        {
            return this.point_.X - this.point.X;
        }
        public int GetVectorY()
        {
            return this.point_.Y - this.point.Y;
        }

        public void SetSource(int x, int y)
        {
            this.isRun = true;
            this.point = new Point(x, y);
            this.point_ = new Point(x, y);
        }
        public void SetTarget(int x, int y)
        {
            if (this.isRun)
            {
                this.point_ = new Point(x, y);
            }
        }
        public void Clear()
        {
            this.isRun = false;
            this.point = new Point();
            this.point_ = new Point();
        }

        public void Draw(Graphics graphics, Pen pen)
        {
            if (this.isRun)
            {
                graphics.DrawLine(pen, this.point, this.point_);
            }
        }
    }
}
