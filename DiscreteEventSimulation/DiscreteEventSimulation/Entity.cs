namespace DiscreteEventSimulation
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

        public void Move()
        {
            this.X += this.VX * this.T;
            this.Y += this.VY * this.T;
            this.T = 0;
        }
    }
}
