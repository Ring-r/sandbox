using System.Windows.Media.Media3D;

namespace MoveOnSphere
{
    public class Entity
    {
        public Vector3D v, v_;
        public float w, w_;
        public float r;

        public void Move()
        {
            Point3D p = new Point3D();

            Quaternion q = new Quaternion(v, w);
            Matrix3D m = new Matrix3D();
            m.RotateAt(q, p);
            v_ = m.Transform(v_);

            Quaternion q_ = new Quaternion(v_, w_);
            Matrix3D m_ = new Matrix3D();
            m_.RotateAt(q_, p);
            v = m_.Transform(v);

        }
    }
}
