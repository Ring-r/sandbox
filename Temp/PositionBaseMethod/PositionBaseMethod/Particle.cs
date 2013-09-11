namespace PositionBasedMethod
{
    public class Particle
    {
        #region Velocity-based

        //Vector3 position;
        //Vector3 velocity;
        //Vector3 acceleration;

        //public void Move(float dt)
        //{
        //    this.position += this.velocity * dt;
        //    this.velocity += this.acceleration * dt;
        //}

        #endregion Velocity-based

        #region Position-based

        public float radius;
        public Vector3 position;
        public Vector3 prevPosition;
        public Vector3 acceleration;

        public void Move(float dt)
        {
            Vector3 delta = this.position - this.prevPosition;
            this.prevPosition = this.position;
            this.position += delta + this.acceleration * dt * dt;
        }

        #endregion Velocity-based
    };
}
