using System;
using System.Collections.Generic;
using System.Drawing;

namespace RegionFilter
{
    class Region
    {
        struct Edge
        {
            private double px, py, vx, vy;

            public Edge(double px, double py, double vx, double vy)
            {
                this.px = px;
                this.py = py;
                double d = Math.Sqrt(vx * vx + vy * vy);
                this.vx = vx / d;
                this.vy = vy / d;
            }

            public double GetY(double x)
            {
                return py + (x - px) * this.vy / this.vx;
            }
            public double Distance(double x, double y)
            {
                return -(x - px) * vy + (y - py) * vx;
            }
        }

        private readonly List<double> pxList = new List<double>();
        private readonly List<List<double>> pyList = new List<List<double>>();
        private readonly List<List<Edge>> edgeList = new List<List<Edge>>();

        private static int BinarySearch(List<double> valueList, double value, out double lastDistance)
        {
            if (valueList.Count == 0)
            {
                lastDistance = 0;
                return -1;
            }

            int begin = 0;
            lastDistance = value - valueList[begin];
            if (lastDistance < 0)
            {
                return -1;
            }

            int end = valueList.Count;
            while (end - begin > 1)
            {
                int middle = (begin + end) / 2;
                lastDistance = value - valueList[middle];
                if (lastDistance >= 0)
                {
                    begin = middle;
                }
                else
                {
                    end = middle;
                }
            }
            lastDistance = value - valueList[begin];
            return begin;
        }
        private static int BinarySearch(List<Edge> edgeList, double x, double y, out double lastDistance)
        {
            if (edgeList.Count == 0)
            {
                lastDistance = 0;
                return -1;
            }

            int begin = 0;
            lastDistance = edgeList[begin].Distance(x, y);
            if (lastDistance < 0)
            {
                return -1;
            }

            int end = edgeList.Count;
            while (end - begin > 1)
            {
                int middle = (begin + end) / 2;
                lastDistance = edgeList[middle].Distance(x, y);
                if (lastDistance >= 0)
                {
                    begin = middle;
                }
                else
                {
                    end = middle;
                }
            }
            lastDistance = edgeList[begin].Distance(x, y);
            return begin;
        }

        private static void UpdateSet(List<int> set, int value)
        {
            if (set.Contains(value))
            {
                set.Remove(value);
            }
            else
            {
                set.Add(value);
            }
        }
        private static void UpdateSet(List<int> set, int value, int maxValue)
        {
            UpdateSet(set, value);
            value = value == 0 ? maxValue : value - 1;
            UpdateSet(set, value);
        }

        private void UpdateLists(int index, List<PointF> pointList, List<int> set)
        {
            int setCount = set.Count;
            int pointListCount_1 = pointList.Count - 1;

            double px = this.pxList[index];
            for (int i = 0; i < setCount; ++i)
            {
                int i0 = set[i];
                int i1 = i0 == pointListCount_1 ? 0 : i0 + 1;

                double vx = pointList[i1].X - pointList[i0].X;
                double vy = pointList[i1].Y - pointList[i0].Y;
                if (vx < 0)
                {
                    vx = -vx;
                    vy = -vy;
                }

                if (vx == 0)
                {
                    this.pyList[index].Add(pointList[i0].Y);
                    this.pyList[index].Add(pointList[i1].Y);
                }
                else
                {
                    double py = pointList[i0].Y + (px - pointList[i0].X) * vy / vx;
                    this.pyList[index].Add(py);
                    this.edgeList[index].Add(new Edge(px, py, vx, vy));
                }
            }
            this.pyList[index].Sort();
            this.pyList[index].TrimExcess();
        }

        public bool Contains(double x, double y)
        {
            double lastDistance;
            int xIndex = BinarySearch(pxList, x, out lastDistance);
            if (xIndex < 0)
            {
                return false;
            }

            int yIndex = lastDistance != 0 ? BinarySearch(this.edgeList[xIndex], x, y, out lastDistance) : BinarySearch(this.pyList[xIndex], y, out lastDistance);
            if (yIndex < 0)
            {
                return false;
            }

            return lastDistance != 0 ? yIndex % 2 == 0 : true;
        }

        public void Update(List<PointF> pointList)
        {
            this.pxList.Clear(); this.pyList.Clear(); this.edgeList.Clear();

            int pointListCount = pointList.Count;
            if (pointListCount > 0)
            {
                List<int> indexList = new List<int>(pointListCount);
                for (int i = 0; i < pointListCount; ++i)
                {
                    indexList.Add(i);
                }
                indexList.Sort((left, right) => (int)(pointList[left].X - pointList[right].X));

                List<int> set = new List<int>(pointListCount); // Or int[] set = new int[pointListCount];?
                UpdateSet(set, indexList[0], pointListCount - 1);

                this.pxList.Add(pointList[indexList[0]].X); this.pyList.Add(new List<double>()); this.edgeList.Add(new List<Edge>());
                for (int i = 1, ii = 0; i < pointListCount; ++i)
                {
                    if (this.pxList[ii] != pointList[indexList[i]].X)
                    {
                        this.UpdateLists(ii, pointList, set);
                        this.pxList.Add(pointList[indexList[i]].X); this.pyList.Add(new List<double>()); this.edgeList.Add(new List<Edge>());
                        ++ii;
                    }
                    UpdateSet(set, indexList[i], pointListCount - 1);
                }

                for (int i = 0; i < this.edgeList.Count - 1; ++i)
                {
                    double middleX = 0.5 * (this.pxList[i] + this.pxList[i + 1]);
                    this.edgeList[i].Sort((left, right) => (int)(left.GetY(middleX) - right.GetY(middleX)));
                    this.edgeList[i].TrimExcess();
                }
            }
        }
    }
}
