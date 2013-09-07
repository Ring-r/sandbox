using System;

namespace MoveOnSphere
{
    public class Vector
    {
        public float x, y, z;

		public float GetLength ()
		{
			return (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
		}

		public void Normilize ()
		{
			float k = 1 / (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
			this.x *= k;
			this.y *= k;
			this.z *= k;
			// this.Multiply(1 / this.GetLength());
		}

		public void Multiply (float scalar)
		{
			this.x *= scalar;
			this.y *= scalar;
			this.z *= scalar;
		}

		public void Add (Vector v)
		{
			this.x += v.x;
			this.y += v.y;
			this.z += v.z;
		}

		public void Deduct (Vector v)
		{
			this.x -= v.x;
			this.y -= v.y;
			this.z -= v.z;
		}

		public void Fill (Vector v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
		}

		/// <summary>
		/// Fills as distinction. this <- (vj - vi)
		/// </summary>
		/// <param name='vi'> Begin point.</param>
		/// <param name='vj'> End point.</param>
		public void FillAsDistinction (Vector vi, Vector vj)
		{
			this.x = vj.x - vi.x;
			this.y = vj.y - vi.y;
			this.z = vj.z - vi.z;
		}
	
		public void FillAsVectorProduction (Vector vi, Vector vj)
		{
			this.x = vi.y * vj.z - vi.z * vj.y;
            this.y = vi.z * vj.x - vi.x * vj.z;
            this.z = vi.x * vj.y - vi.y * vj.x;
		}
	
		public static float ScalarProduction (Vector vi, Vector vj)
		{
			return vi.x * vj.x + vi.y * vj.y + vi.z * vj.z;
		}
		public static float Angle(Vector vi, Vector vj)
		{
			return (float)Math.Acos(vi.x * vj.x + vi.y * vj.y + vi.z * vj.z);
			// return (float)Math.Acos(Vector.ScalarProduction(vi, vj));
		}
	}
}
