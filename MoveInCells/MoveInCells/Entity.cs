namespace MoveInCells
{
    public class Entity
    {
        public int Id;

        public float X, Y; // Coordinates.
        public float V, A; // Speed and rotate angle.
        public float VX, VY; // Moving vector.
        public void SetV(float v, float a)
        {
            this.V = v;
            this.A = a;
            this.VX = v * (float)System.Math.Cos(a);
            this.VY = v * (float)System.Math.Sin(a);
        }

        public float R; // Radius.

        public float T; // Time.

        public int i, j, k; // Cell indexes.

        public int Event; // Next event type.
        public Entity Next; // Next collision event entity.
        public Entity Near; // Near entity to catch.

        public float Score;
        public State State;

        public void Move(float t)
        {
            this.X += this.VX * t;
            this.Y += this.VY * t;
            this.T -= t;
        }
    }
}
