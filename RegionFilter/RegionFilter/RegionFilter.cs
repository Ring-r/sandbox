using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace RegionFilter
{
    public class RegionFilter : IRegionFilter
    {
        private double[] xs = new double[0];
        private double[][][] xEdges = new double[0][][];

        public void Init(List<Point3D> pointList)
        {
            this.Init(new List<List<Point3D>> { pointList });
        }

        public void Init(List<List<Point3D>> pointListList)
        {
            var sortedSet = new SortedSet<double>();
            pointListList.ForEach(pointList => pointList.ForEach(point => sortedSet.Add(point.X)));

            this.xs = sortedSet.ToArray();
            this.xEdges = new double[this.xs.Length][][];

            var xEdgeList = new List<double[]>[this.xs.Length];
            for (var i = 0; i < xEdgeList.Length; ++i)
            {
                xEdgeList[i] = new List<double[]>();
            }

            foreach (var pointList in pointListList)
            {
                var indexList = pointList.Select(point => Array.BinarySearch(this.xs, point.X) - 1).ToList();

                for (int i = pointList.Count - 1, j = 0; j < pointList.Count; i = j, ++j)
                {
                    if (pointList[i].X == pointList[j].X)
                    {
                        continue;
                    }
                    var iMin = pointList[i].X < pointList[j].X ? i : j;
                    var iMax = i + j - iMin;
                    var x1 = pointList[iMin].X;
                    var y1 = pointList[iMin].Y;
                    var x2 = pointList[iMax].X;
                    var y2 = pointList[iMax].Y;
                    var slope = (y2 - y1) / (x2 - x1);

                    var xIndex1 = indexList[iMin];
                    var xIndex2 = indexList[iMax];

                    for (var xIndex = xIndex1 + 1; xIndex <= xIndex2; ++xIndex)
                    {
                        xEdgeList[xIndex].Add(new[] { y1 + (this.xs[xIndex] - x1) * slope, slope });
                    }
                }
            }

            for (var xIndex = 0; xIndex < xEdgeList.Length; ++xIndex)
            {
                xEdgeList[xIndex].Sort((t1, t2) => Math.Sign(t1[0] == t2[0] ? t1[1] - t2[1] : t1[0] - t2[0]));
                this.xEdges[xIndex] = xEdgeList[xIndex].ToArray();
            }
        }

        public bool Contains(double x, double y)
        {
            var xBegin = -1;
            var xEnd = this.xs.Length;
            while (xEnd - xBegin > 1)
            {
                var xMiddle = (xEnd + xBegin) >> 1;
                if (this.xs[xMiddle] < x)
                    xBegin = xMiddle;
                else
                    xEnd = xMiddle;
            }
            if (xBegin < 0)
            {
                return false;
            }

            var dx = x - this.xs[xBegin];
            var edges = this.xEdges[xBegin];
            var edgeBegin = -1;
            var edgeEnd = edges.Length;
            while (edgeEnd - edgeBegin > 1)
            {
                var edgeMiddle = (edgeEnd + edgeBegin) >> 1;
                if (edges[edgeMiddle][0] + dx * edges[edgeMiddle][1] < y)
                    edgeBegin = edgeMiddle;
                else
                    edgeEnd = edgeMiddle;
            }
            if (edgeBegin < 0)
            {
                return false;
            }

            if (edges[edgeBegin][0] + dx * edges[edgeBegin][1] == y)
            {
                return true;
            }

            return edgeBegin % 2 == 0;
        }
    }
}
