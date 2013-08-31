namespace MoveInCells
{
    public class Entity
    {
        public int Id;

        public float X, Y; // Coordinates.
		public float V, A; // Speed and rotate angle.
		public float VX, VY; // Moving vector.
		public void SetV (float v, float a)
		{
			this.V = v;
			this.A = a;
			this.VX = v * (float)System.Math.Cos(a);
			this.VY = v * (float)System.Math.Sin(a);
		}

		public float XT, YT; // Temp (or local) coordinates.

		public float R; // Radius.

        public float T; // Time.

        public int i, j; // Cell indexes.

        public int Event; // Next event type.
        public Entity Next; // Next collision event entity.

        public State State;
        public float M; // Mana. Score.

        public void Move(float t)
        {
            this.X += this.VX * t;
            this.Y += this.VY * t;
            this.T -= t;
        }
    }

    public enum State { Run, Freeze, Catch };
}
