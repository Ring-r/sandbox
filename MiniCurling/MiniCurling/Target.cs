using System;
using System.Drawing;

namespace MiniCurling
{
    public class Target : Circle
    {
        public Target(float radius)
            : base(radius)
        {
        }

        public new void Draw(Graphics graphics, Brush brush)
        {
            graphics.DrawEllipse(Pens.Red, this.px - this.radius, this.py - this.radius, 2 * this.radius, 2 * this.radius);
            graphics.FillEllipse(Brushes.Red, this.px - 0.5f * this.radius, this.py - 0.5f * this.radius, this.radius, this.radius);
        }
    }
}
