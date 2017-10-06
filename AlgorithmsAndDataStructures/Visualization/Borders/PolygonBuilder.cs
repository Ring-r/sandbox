using System;
using System.Collections.Generic;
using AlgorithmsAndDataStructures;

namespace Buildings
{
    public class PolygonBuilder
    {
        public readonly List<Vector3d> points = new List<Vector3d>();
        public readonly List<int> contour = new List<int>();
        public List<Tuple<int, int, int>> triangles = null;

        private static double Distance2D(Vector3d loc1, Vector3d loc2)
        {
            double dX = loc1.X - loc2.X;
            double dY = loc1.Y - loc2.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }

        public void Clear()
        {
            this.points.Clear();
            this.contour.Clear();
        }

        public void Init(List<Vector3d> points)
        {
            this.points.AddRange(points);
        }

        public IEnumerable<bool> BuildContour(double radius)
        {
            this.contour.Clear();

            int currIndex = -1;
            #region Find first point.

            double minX = double.MaxValue;
            for (int i = 0; i < this.points.Count; ++i)
            {
                if (minX > this.points[i].X)
                {
                    minX = this.points[i].X;
                    currIndex = i;
                }
            }

            #endregion Find first point.
            this.contour.Add(currIndex);

            Vector3d lastVector = new Vector3d(0, 1, 0);
            Vector3d currPoint = this.points[currIndex];

            bool isExit = false;
            do
            {
                int index = -1;
                double glCos = double.MinValue;
                double glLength = double.MinValue;
                for (int i = 0; i < this.points.Count; ++i)
                {
                    if (i != currIndex && Distance2D(this.points[i], currPoint) <= radius)
                    {
                        Vector3d vec = this.points[i] - currPoint;
                        double cos = (vec.X * lastVector.X + vec.Y * lastVector.Y) / vec.Length2D() / lastVector.Length2D();
                        double sign = vec.X * lastVector.Y - vec.Y * lastVector.X;
                        if (sign > 0 || (sign == 0 && cos > 0))
                        {
                            cos = -cos - 2;
                        }
                        double length = vec.Length2D();

                        if (cos < 0.5 && glCos < cos || (glCos == cos && glLength < length))
                        {
                            index = i;
                            glCos = cos;
                            glLength = length;
                        }
                    }
                }
                currIndex = index;
                this.contour.Add(currIndex);

                yield return true;

                lastVector = currPoint - this.points[currIndex];
                currPoint = this.points[currIndex];

            } while (currIndex != this.contour[0] && !isExit);
            //this.contour.RemoveAt(this.contour.Count - 1); // Last vertice = first vertice
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
                    Vector3d vec1 = this.points[pointsToProcess[lastIndex]] - this.points[pointsToProcess[i]];
                    Vector3d vec2 = this.points[pointsToProcess[i + 1]] - this.points[pointsToProcess[i]];
                    if (vec1.CrossProduct2D(vec2) > 0)
                    {
                        // Check that there are no polygon vertices inside
                        Vector3d v1 = this.points[pointsToProcess[i]];
                        Vector3d v2 = this.points[pointsToProcess[i + 1]];
                        Vector3d v3 = this.points[pointsToProcess[lastIndex]];
                        bool inside = false;
                        for (int j = 0; j < pointsToProcess.Count; ++j)
                        {
                            if ((j == i) || (j == i + 1) || (j == lastIndex))
                            {
                                continue;
                            }

                            Vector3d pt = this.points[pointsToProcess[j]];
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
            Vector3d lvec1 = this.points[pointsToProcess[2]] - this.points[pointsToProcess[0]];
            Vector3d lvec2 = this.points[pointsToProcess[1]] - this.points[pointsToProcess[0]];
            if (lvec1.CrossProduct2D(lvec2) > 0)
            {
                this.triangles.Add(new Tuple<int, int, int>(pointsToProcess[0], pointsToProcess[1], pointsToProcess[2]));
            }
        }
    }
}
