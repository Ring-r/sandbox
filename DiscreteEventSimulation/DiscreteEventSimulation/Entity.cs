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

        public float Time; // Time.

        public int i, j, k; // Cell indexes.

        public int Event; // Next event type.
        public Entity Next; // Next collision event entity.

        public void Update(float time)
        {
            this.X += time * this.VX;
            this.Y += time * this.VY;
            this.Time -= time;
        }
    }
}
