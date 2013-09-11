using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PositionBasedMethod
{
    public struct Vector3
    {
        public float x, y, z;
        public static Vector3 operator *(Vector3 vector, float scalar)
        {
            return new Vector3() { x = vector.x * scalar, y = vector.y * scalar, z = vector.z * scalar };
        }
        public static Vector3 operator +(Vector3 vector_i, Vector3 vector_j)
        {
            return new Vector3() { x = vector_i.x + vector_j.x, y = vector_i.y + vector_j.y, z = vector_i.z + vector_j.z };
        }
        public static Vector3 operator -(Vector3 vector_i, Vector3 vector_j)
        {
            return new Vector3() { x = vector_i.x - vector_j.x, y = vector_i.y - vector_j.y, z = vector_i.z - vector_j.z };
        }
        public float SquareLength()
        {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        }
        public float Length()
        {
            return (float)Math.Sqrt(this.SquareLength());
        }
        public void Normalize()
        {
            float k = 1 / this.Length();
            this.x *= k;
            this.y *= k;
            this.z *= k;
        }
    }
}
