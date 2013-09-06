using System;

namespace MoveOnSphere
{
    public class Quaternion
    {
        private float x, y, z, w;

        public Quaternion(float x, float y, float z, float a)
        {
            float sin = (float)Math.Sin(0.5 * a);
            this.x = sin * x;
            this.y = sin * y;
            this.z = sin * z;
            this.w = (float)Math.Cos(0.5 * a);
        }
        public Quaternion(Vector v, float a)
        {
            float sin = (float)Math.Sin(0.5 * a);
            this.x = sin * v.x;
            this.y = sin * v.y;
            this.z = sin * v.z;
            this.w = (float)Math.Cos(0.5 * a);
        }
        public Quaternion(Vector v1, Vector v2)
        {
            float cosA = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
            float sinA_2 = (float)Math.Sqrt(0.5 * (1 - cosA));
            float cosA_2 = (float)Math.Sqrt(0.5 * (1 + cosA));

            this.x = (v1.y * v2.z - v1.z * v2.y) * sinA_2;
            this.y = (v1.z * v2.x - v1.x * v2.z) * sinA_2;
            this.z = (v1.x * v2.y - v1.y * v2.x) * sinA_2;
            this.w = cosA_2;
        }

        public void Convert(ref float x, ref float y, ref float z)
        {
            float wx, wy, wz, xx, yy, yz, xy, xz, zz, x2, y2, z2;
            x2 = this.x + this.x;
            y2 = this.y + this.y;
            z2 = this.z + this.z;
            xx = this.x * x2; xy = this.x * y2; xz = this.x * z2;
            yy = this.y * y2; yz = this.y * z2; zz = this.z * z2;
            wx = this.w * x2; wy = this.w * y2; wz = this.w * z2;

            float a00, a01, a02;
            a00 = 1.0f - (yy + zz); a01 = xy - wz; a02 = xz + wy;
            float a10, a11, a12;
            a10 = xy + wz; a11 = 1.0f - (xx + zz); a12 = yz - wx;
            float a20, a21, a22;
            a20 = xz - wy; a21 = yz + wx; a22 = 1.0f - (xx + yy);

            float nx = a00 * x + a01 * y + a02 * z;
            float ny = a10 * x + a11 * y + a12 * z;
            float nz = a20 * x + a21 * y + a22 * z;

            x = nx; y = ny; z = nz;
        }
        public void Convert(Vector v)
        {
            this.Convert(ref v.x, ref v.y, ref v.z);
        }
    }
}
