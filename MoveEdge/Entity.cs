using System;
using System.Drawing;

namespace Prototype
{
    public class Entity
    {
        public float r = 10;
        public float x_, _x, y_, _y;
        public float vx_, _vx, vy_, _vy;

        public void Init(float x, float y, float a = 0)
        {
            x_ = x - r * (float)Math.Cos(a);
            _x = x + r * (float)Math.Cos(a);
            y_ = y - r * (float)Math.Sin(a);
            _y = y + r * (float)Math.Sin(a);
        }

        public void Move()
        {
            float a = (float)Math.Atan2(_y - y_, _x - x_);

            x_ += vx_ * (float)Math.Cos(a) - vy_ * (float)Math.Sin(a);
            y_ += vx_ * (float)Math.Sin(a) + vy_ * (float)Math.Cos(a);

            _x += _vx * (float)Math.Cos(a) - _vy * (float)Math.Sin(a);
            _y += _vx * (float)Math.Sin(a) + _vy * (float)Math.Cos(a);

            float px = 0.5f * (x_ + _x);
            float py = 0.5f * (y_ + _y);

            float vx = _x - x_;
            float vy = _y - y_;
            float k = r / (float)Math.Sqrt(vx * vx + vy * vy);
            vx *= k; vy *= k;

            x_ = px - vx; y_ = py - vy;
            _x = px + vx; _y = py + vy;
        }

        public void Draw(Graphics g, Pen pen)
        {
            g.DrawLine(pen, x_, y_, _x, _y);
        }
    }
}
