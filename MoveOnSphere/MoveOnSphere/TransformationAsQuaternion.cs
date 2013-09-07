using System;

namespace MoveOnSphere
{
    public class TransformationAsQuaternion
    {
        private  float x, y, z, w;
		private float a00, a01, a02;
        private float a10, a11, a12;
        private float a20, a21, a22;

		private void UpdateMatrix ()
		{
            float wx, wy, wz, xx, yy, yz, xy, xz, zz, x2, y2, z2;
            x2 = this.x + this.x;
            y2 = this.y + this.y;
            z2 = this.z + this.z;
            xx = this.x * x2; xy = this.x * y2; xz = this.x * z2;
            yy = this.y * y2; yz = this.y * z2; zz = this.z * z2;
            wx = this.w * x2; wy = this.w * y2; wz = this.w * z2;

            this.a00 = 1.0f - (yy + zz); this.a01 = xy - wz; this.a02 = xz + wy;
            this.a10 = xy + wz; this.a11 = 1.0f - (xx + zz); this.a12 = yz - wx;
            this.a20 = xz - wy; this.a21 = yz + wx; this.a22 = 1.0f - (xx + yy);
		}

        public void Fill(Vector v, float a)
        {
            float sin = (float)Math.Sin(0.5 * a);
            this.x = sin * v.x;
            this.y = sin * v.y;
            this.z = sin * v.z;
            this.w = (float)Math.Cos(0.5 * a);

			this.UpdateMatrix();
        }
        public void Fill(Vector v, Vector v1, Vector v2)
		{
			float cosA = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;

			float sinA_2 = (float)Math.Sqrt (0.5 * (1 - cosA));
            this.x = v.x * sinA_2;
            this.y = v.y * sinA_2;
            this.z = v.z * sinA_2;
            this.w = (float)Math.Sqrt (0.5 * (1 + cosA));

			this.UpdateMatrix();
        }
        public void Fill (Vector v1, Vector v2)
		{
			float cosA = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
			float sinA_2 = (float)Math.Sqrt (0.5 * (1 - cosA));
			float cosA_2 = (float)Math.Sqrt (0.5 * (1 + cosA));

			this.w = cosA_2;
			if (this.w != 0) {
				this.x = (v1.y * v2.z - v1.z * v2.y);
				this.y = (v1.z * v2.x - v1.x * v2.z);
				this.z = (v1.x * v2.y - v1.y * v2.x);
				float d = sinA_2 / (float)Math.Sqrt (this.x * this.x + this.y * this.y + this.z * this.z);
				this.x *= d;
				this.y *= d;
				this.z *= d;
			} else {
				this.x = 0;
				this.y = 0;
				this.z = 1;
			}
			this.UpdateMatrix();
        }

        public void Transform(Vector v)
        {
            float nx = this.a00 * v.x + this.a01 * v.y + this.a02 * v.z;
            float ny = this.a10 * v.x + this.a11 * v.y + this.a12 * v.z;
            float nz = this.a20 * v.x + this.a21 * v.y + this.a22 * v.z;

            v.x = nx; v.y = ny; v.z = nz;
        }
    }
}
