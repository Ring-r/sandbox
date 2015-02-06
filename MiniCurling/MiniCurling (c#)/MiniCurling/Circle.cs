using System;
using System.Drawing;

namespace MiniCurling
{
    public abstract class Circle
    {
        protected float radius;
        protected float px, py;

        public Circle(float radius)
        {
            this.radius = radius;
        }

        public void Init(Random random, float maxX, float maxY)
        {
            this.px = this.radius + (maxX - 2 * this.radius) * (float)random.NextDouble();
            this.py = this.radius + (maxY - 2 * this.radius) * (float)random.NextDouble();
        }

        public void Draw(Graphics graphics, Brush brush)
        {
            graphics.FillEllipse(brush, this.px - this.radius, this.py - this.radius, 2 * this.radius, 2 * this.radius);
        }

        public static double Distance(Circle source, Circle target)
        {
            double x = target.px - source.px;
            double y = target.py - source.py;
            return Math.Sqrt(x * x + y * y) - target.radius - source.radius;
        }
    }
}
