namespace MoveInCells
{
    public class Entity
    {
        public int Id;

        public float X;
        public float VX;

        public float Y;
        public float VY;

        public float R; // Radius.

        public float T; // Time.

        public int i; // Cell index.
        public int j; // Cell index.

        public Entity Next; // Next collision event entity.

        public int Event; // Next event type.

        public State State;
        public float M; // Mana. Score.

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

    public enum State { Run, Freeze, Catch };
}
