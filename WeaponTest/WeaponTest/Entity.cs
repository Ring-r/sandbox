using System;
using System.Drawing;

namespace Entities
{
    abstract class Entity : IEntity
    {
        public float Angle { get; set; }
        public float VectorX
        {
            get
            {
                return (float)Math.Cos(this.Angle);
            }
        }
        public float VectorY
        {
            get
            {
                return (float)Math.Sin(this.Angle);
            }
        }
        public float Speed { get; set; }

        public Pen Pen = Pens.Black;
        public Brush Brush = Brushes.Red;

        public abstract void onManagedDraw(Graphics graphics);
        public abstract void onManagedUpdate(float pSecondsElapsed);

    }
}
