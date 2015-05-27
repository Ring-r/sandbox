using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace RegionFilter
{
	public class RegionFilter: IRegionFilter
	{
		private double[] xs = new double[0];
		private double[][][] xEdges = new double[0][][];

		public void Update(List<List<Point3D>> pointListList)
		{
			SortedSet<double> sortedSet = new SortedSet<double>();
			pointListList.ForEach(pointList => pointList.ForEach(point => sortedSet.Add(point.X)));

			this.xs = sortedSet.ToArray();
			this.xEdges = new double[this.xs.Length][][];

			List<double[]>[] xEdgeList = new List<double[]>[this.xs.Length];
			for (int i = 0; i < xEdgeList.Length; ++i)
			{
				xEdgeList[i] = new List<double[]>();
			}

			foreach (List<Point3D> pointList in pointListList)
			{
				List<int> indexList = new List<int>(pointList.Count);
				for (int j = 0; j < pointList.Count; ++j)
				{
					indexList.Add(Array.BinarySearch(this.xs, pointList[j].X) - 1);
				}
				for (int i = pointList.Count - 1, j = 0; j < pointList.Count; i = j, ++j)
				{
					if (pointList[i].X == pointList[j].X)
					{
						continue;
					}
					int iMin = pointList[i].X < pointList[j].X ? i : j;
					int iMax = i + j - iMin;
					double x1 = pointList[iMin].X;
					double y1 = pointList[iMin].Y;
					double x2 = pointList[iMax].X;
					double y2 = pointList[iMax].Y;
					double slope = (y2 - y1) / (x2 - x1);

					int xIndex1 = indexList[iMin];
					int xIndex2 = indexList[iMax];

					for (int xIndex = xIndex1 + 1; xIndex <= xIndex2; ++xIndex)
					{
						xEdgeList[xIndex].Add(new double[2] { y1 + (this.xs[xIndex] - x1) * slope, slope });
					}
				}
			}

			for (int xIndex = 0; xIndex < xEdgeList.Length; ++xIndex)
			{
				xEdgeList[xIndex].Sort((t1, t2) => Math.Sign(t1[0] == t2[0] ? t1[1] - t2[1] : t1[0] - t2[0]));
				this.xEdges[xIndex] = xEdgeList[xIndex].ToArray();
			}
		}

		public void Update(List<Point3D> pointListList)
		{
			this.Update(pointListList.AsEnumerable().ToList());
		}

		public bool Contains(double x, double y)
		{
			int xBegin = -1;
			int xEnd = this.xs.Length;
			int xMiddle;
			while (xEnd - xBegin > 1)
			{
				xMiddle = (xEnd + xBegin) >> 1;
				if (this.xs[xMiddle] < x)
					xBegin = xMiddle;
				else
					xEnd = xMiddle;
			}
			if (xBegin < 0)
			{
				return false;
			}

			double dx = x - this.xs[xBegin];
			double[][] edges = this.xEdges[xBegin];
			int edgeBegin = -1;
			int edgeEnd = edges.Length;
			int edgeMiddle;
			while (edgeEnd - edgeBegin > 1)
			{
				edgeMiddle = (edgeEnd + edgeBegin) >> 1;
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
