namespace MoveOnSphere
{
    public class Entity
    {
        public float x, y, z, a;
        public float x_, y_, z_, a_;

        public void Move()
        {
            Quaternion q = new Quaternion(x, y, z, a);
			q.Convert(ref x_, ref y_, ref z_);

            Quaternion q_ = new Quaternion(x_, y_, z_, a_);
			q_.Convert(ref x, ref y, ref z);
        }
    }
}
