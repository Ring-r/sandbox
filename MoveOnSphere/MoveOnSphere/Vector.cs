using System;

namespace MoveOnSphere
{
    public class Vector
    {
        public float x, y, z;

        public static Vector CrossProductAndNormilize(Vector v1, Vector v2)
        {
            Vector v = new Vector();
            v.x = v1.y * v2.z - v1.z * v2.y;
            v.y = v1.z * v2.x - v1.x * v2.z;
            v.z = v1.x * v2.y - v1.y * v2.x;
            float k = 1 / (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            v.x *= k;
			v.y *= k;
			v.z *= k;
            return v;
        }
    }
}
