using System;

namespace AlgorithmsAndDataStructures
{
    public class Vector3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3d operator -(Vector3d loc1, Vector3d loc2)
        {
            return new Vector3d(loc1.X - loc2.X, loc1.Y - loc2.Y, loc1.Z - loc2.Z);
        }

        public double DotProduct2D(Vector3d loc)
        {
            return this.X * loc.X + this.Y * loc.Y;
        }

        public double CrossProduct2D(Vector3d loc)
        {
            return this.X * loc.Y - this.Y * loc.X;
        }

        public static Vector3d operator *(double scalar, Vector3d loc)
        {
            return new Vector3d(scalar * loc.X, scalar * loc.Y, scalar * loc.Z);
        }

        public double Length2D()
        {
            return Math.Sqrt(this.X * this.X + this.Y * this.Y);
        }
    }
}
