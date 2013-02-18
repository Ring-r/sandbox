using System.Drawing;

namespace MoveInCells
{
    class Entity
    {
        public float X;
        public float Y;
        public float R;

        public float VX;
        public float VY;

        public float T;
        public float TLoc;

        public int i;
        public int j;

        public Entity Next;

        public int Event;

        public int Id;
        public Brush Brush;

        public void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}
