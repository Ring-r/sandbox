using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Buildings
{
    public class PolygonBuilder
    {
        public List<LocatorZ> points;
        public List<int> contour;
        public List<Tuple<int, int, int>> triangles;

        public double Distance2D(LocatorZ loc1, LocatorZ loc2)
        {
            double dX = loc1.X - loc2.X;
            double dY = loc1.Y - loc2.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }

        public PolygonBuilder(List<LocatorZ> points)
        {
            this.points = points;
            this.contour = null;
            this.triangles = null;
        }

        public void BuildContour(double radius)
        {
            // 0. Preparations
            int pointCount = this.points.Count;
            this.contour = new List<int>();

            // 1. Get point with lowest X and mark it as origin
            double minX = double.MaxValue;
            int lastIndex = -1;
            for (int i = 0; i < pointCount; ++i)
            {
                if (this.points[i].X < minX)
                {
                    lastIndex = i;
                    minX = this.points[i].X;
                }
            }
            this.contour.Add(lastIndex);

            LocatorZ lastVector = new LocatorZ(0, 1, 0);
            // 2. Start wrapping with small radius:
            do
            {
                LocatorZ lastPoint = this.points[lastIndex];

                int index = -1;
                double glDist = double.MaxValue;
                double glLength = double.MinValue;
                for (int i = 0; i < pointCount; ++i)
                {
                    if (i != lastIndex && this.Distance2D(this.points[i], lastPoint) <= radius)
                    {
                        LocatorZ vec = this.points[i] - lastPoint;
                        double dist = vec.X * lastVector.Y - vec.Y * lastVector.X;
                        double length = vec.Length2D();

                        if (dist >= 0 && (glDist > dist || (glDist == dist && glLength < length)))
                        {
                            index = i;
                            glDist = dist;
                            glLength = length;
                        }
                    }
                }
                lastIndex = index;
                lastVector = this.points[lastIndex] - lastPoint;
                this.contour.Add(lastIndex);

                // Check if contour closed
            } while (lastIndex != this.contour[0]);
            this.contour.RemoveAt(this.contour.Count - 1); // Last vertice = first vertice
        }

        public void TriangulateContour()
        {
            //algorithm info - http://www.opita.net/node/12
            List<int> pointsToProcess = new List<int>();
            for (int i = 0; i < this.contour.Count; ++i)
            {
                pointsToProcess.Add(this.contour[i]);
            }

            this.triangles = new List<Tuple<int, int, int>>();
            while (pointsToProcess.Count > 3)
            {
                for (int i = 0; i < pointsToProcess.Count - 1; ++i)
                {
                    // Check vectors to form left-handed pair (positive cross-product)
                    int lastIndex = i + 2 < pointsToProcess.Count ? i + 2 : 0;
                    LocatorZ vec1 = this.points[pointsToProcess[lastIndex]] - this.points[pointsToProcess[i]];
                    LocatorZ vec2 = this.points[pointsToProcess[i + 1]] - this.points[pointsToProcess[i]];
                    if (vec1.CrossProduct2D(vec2) > 0)
                    {
                        // Check that there are no polygon vertices inside
                        LocatorZ v1 = this.points[pointsToProcess[i]];
                        LocatorZ v2 = this.points[pointsToProcess[i + 1]];
                        LocatorZ v3 = this.points[pointsToProcess[lastIndex]];
                        bool inside = false;
                        for (int j = 0; j < pointsToProcess.Count; ++j)
                        {
                            if ((j == i) || (j == i + 1) || (j == lastIndex))
                            {
                                continue;
                            }

                            LocatorZ pt = this.points[pointsToProcess[j]];
                            double cp1 = (v1.X - pt.X) * (v2.Y - v1.Y) - (v2.X - v1.X) * (v1.Y - pt.Y);
                            double cp2 = (v2.X - pt.X) * (v3.Y - v2.Y) - (v3.X - v2.X) * (v2.Y - pt.Y);
                            double cp3 = (v3.X - pt.X) * (v1.Y - v3.Y) - (v1.X - v3.X) * (v3.Y - pt.Y);
                            if (((cp1 >= 0) && (cp2 >= 0) && (cp3 >= 0)) || ((cp1 <= 0) && (cp2 <= 0) && (cp3 <= 0)))
                            {
                                inside = true;
                                break;
                            }
                        }

                        if (inside)
                        {
                            continue;
                        }

                        // if there are no points inside - add this triangle and subtract it from the contour
                        this.triangles.Add(new Tuple<int, int, int>(pointsToProcess[i], pointsToProcess[i + 1], pointsToProcess[lastIndex]));
                        pointsToProcess.RemoveAt(i + 1);
                        break;
                    }
                }
            }
            // Add last three points as the last triangle if they form left-handed pair
            LocatorZ lvec1 = this.points[pointsToProcess[2]] - this.points[pointsToProcess[0]];
            LocatorZ lvec2 = this.points[pointsToProcess[1]] - this.points[pointsToProcess[0]];
            if (lvec1.CrossProduct2D(lvec2) > 0)
            {
                this.triangles.Add(new Tuple<int, int, int>(pointsToProcess[0], pointsToProcess[1], pointsToProcess[2]));
            }
        }
    }
}
