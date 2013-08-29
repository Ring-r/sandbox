using System.Drawing;

namespace MoveInCells
{
    class Entity
    {
        public float X;
        public float VX;

        public float Y;
        public float VY;

        public float R;

        public float T;

        public int i;
        public int j;

        public Entity Next;

        public int Event;

        public int Id;
        public Brush Brush;
        public bool D;
        public float M;

        public void Move()
        {
            this.X += this.VX * this.T;
            this.Y += this.VY * this.T;
        }

        public void Move(float t)
        {
            this.X += this.VX * t;
            this.Y += this.VY * t;
            this.T -= t;
        }
    }
}
