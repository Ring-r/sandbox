using System;

namespace Buildings
{
    public class LocatorZ
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public LocatorZ(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static LocatorZ operator -(LocatorZ loc1, LocatorZ loc2)
        {
            return new LocatorZ(loc1.X - loc2.X, loc1.Y - loc2.Y, loc1.Z - loc2.Z);
        }

        public double DotProduct2D(LocatorZ loc)
        {
            return this.X * loc.X + this.Y * loc.Y;
        }

        public double CrossProduct2D(LocatorZ loc)
        {
            return this.X * loc.Y - this.Y * loc.X;
        }

        public static LocatorZ operator *(double scalar, LocatorZ loc)
        {
            return new LocatorZ(scalar * loc.X, scalar * loc.Y, scalar * loc.Z);
        }

        public double Length2D()
        {
            return Math.Sqrt(this.X * this.X + this.Y * this.Y);
        }
    }
}
