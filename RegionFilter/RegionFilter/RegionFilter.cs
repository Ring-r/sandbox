using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace RegionFilter
{
	public interface IRegionFilter
	{
		void Update(List<List<Point3D>> pointListList);
		bool Contains(double x, double y);
	}

	public class RegionFilter: IRegionFilter
	{
		class Edge
		{
			public double BeginX { get; private set; }
			public double EndX { get; private set; }

			public double BeginY { get; private set; }
			public double EndY { get; private set; }

			public Edge(double beginX, double beginY, double endX, double endY)
			{
				if (beginX <= endX)
				{
					this.BeginX = beginX;
					this.BeginY = beginY;
					this.EndX = endX;
					this.EndY = endY;
				}
				else
				{
					this.BeginX = endX;
					this.BeginY = endY;
					this.EndX = beginX;
					this.EndY = beginY;
				}
			}
		}

		class Line
		{
			private double px;
			private double py;
			private double vx;
			private double vy;

			public Line(double px, double py, double vx, double vy)
			{
				this.px = px;
				this.py = py;

				this.vx = vx;
				this.vy = vy;
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
		private readonly List<List<Line>> lineList = new List<List<Line>>();

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
		private static int BinarySearch(List<Line> lineList, double x, double y, out double lastDistance)
		{
			if (lineList.Count == 0)
			{
				lastDistance = 0;
				return -1;
			}

			int begin = 0;
			lastDistance = lineList[begin].Distance(x, y);
			if (lastDistance < 0)
			{
				return -1;
			}

			int end = lineList.Count;
			while (end - begin > 1)
			{
				int middle = (begin + end) / 2;
				lastDistance = lineList[middle].Distance(x, y);
				if (lastDistance >= 0)
				{
					begin = middle;
				}
				else
				{
					end = middle;
				}
			}
			lastDistance = lineList[begin].Distance(x, y);
			return begin;
		}

		private List<Edge> CreateEdges(List<List<Point3D>> pointListList)
		{
			List<Edge> edges = new List<Edge>();
			SortedSet<double> sortedSet = new SortedSet<double>();
			foreach (List<Point3D> pointList in pointListList)
			{
				int pointListCount = pointList.Count;
				if (pointListCount >= 3)
				{
					for (int j = 0; j < pointListCount; ++j)
					{
						if (!sortedSet.Contains(pointList[j].X))
						{
							sortedSet.Add(pointList[j].X);
						}
						int j_next = j < pointListCount - 1 ? j + 1 : 0;
						edges.Add(new Edge(pointList[j].X, pointList[j].Y, pointList[j_next].X, pointList[j_next].Y));
					}
				}
			}
			this.pxList.AddRange(sortedSet);
			return edges;
		}

		private void FillStrips(List<Edge> edges)
		{
			foreach (double x in this.pxList)
			{
				this.pyList.Add(new List<double>());
				List<double> pyList = this.pyList[this.pyList.Count - 1];
				this.lineList.Add(new List<Line>());
				List<Line> lineList = this.lineList[this.lineList.Count - 1];
				foreach (Edge edge in edges)
				{
					if (x == edge.BeginX)
					{
						pyList.Add(edge.BeginY);
					}
					if (x == edge.EndX)
					{
						pyList.Add(edge.EndY);
					}

					if (edge.BeginX <= x && x < edge.EndX)
					{
						Line line = new Line(edge.BeginX, edge.BeginY, edge.EndX - edge.BeginX, edge.EndY - edge.BeginY);
						lineList.Add(line);
						pyList.Add(line.GetY(x));
					}
				}
				pyList.Sort();
				pyList.TrimExcess();
			}

			int pxListCount = this.pxList.Count;
			for (int i = 0; i < pxListCount - 1; ++i)
			{
				double middleX = 0.5 * (this.pxList[i] + this.pxList[i + 1]);
				this.lineList[i].Sort((left, right) => Math.Sign(left.GetY(middleX) - right.GetY(middleX)));
				this.lineList[i].TrimExcess();
			}
		}

		public void Update(List<List<Point3D>> pointListList)
		{
			this.pxList.Clear();
			this.pyList.Clear();
			this.lineList.Clear();
			if (pointListList != null)
			{
				this.FillStrips(this.CreateEdges(pointListList)); // TODO: Not optimal algorithm of building data.
			}
		}

		public bool Contains(double x, double y)
		{
			if (this.pxList.Count == 0)
			{
				return true;
			}

			double lastDistance;
			int xIndex = BinarySearch(pxList, x, out lastDistance);
			if (xIndex < 0)
			{
				return false;
			}

			int yIndex = lastDistance != 0 ? BinarySearch(this.lineList[xIndex], x, y, out lastDistance) : BinarySearch(this.pyList[xIndex], y, out lastDistance);
			if (yIndex < 0)
			{
				return false;
			}

			return lastDistance != 0 ? yIndex % 2 == 0 : true;
		}
	}

	public class RegionFilterOther: IRegionFilter
	{
		private readonly List<double> pxList = new List<double>();
		private readonly List<List<Tuple<double, double>>> pyListList = new List<List<Tuple<double, double>>>();

		private static int BinarySearch<T>(List<T> valueList, Func<T, double> f, double value)
		{
			int begin = -1;
			int end = valueList.Count;
			int middle;
			while (end - begin > 1)
			{
				middle = (end + begin) >> 1;
				if (f(valueList[middle]) < value)
					begin = middle;
				else
					end = middle;
			}
			return begin;
		}

		private void FillStrips(IEnumerable<List<Point3D>> pointListList)
		{
			// fill sorted x list
			SortedSet<double> sortedSet = new SortedSet<double>();
			foreach (List<Point3D> pointList in pointListList)
			{
				for (int j = 0; j < pointList.Count; ++j)
				{
					sortedSet.Add(pointList[j].X);
				}
			}
			this.pxList.AddRange(sortedSet);
			foreach (double x in this.pxList)
			{
				this.pyListList.Add(new List<Tuple<double, double>>());
			}

			// fill y and slope lists
			foreach (List<Point3D> pointList in pointListList)
			{
				List<int> indexList = new List<int>();
				for (int j = 0; j < pointList.Count; ++j)
				{
					indexList.Add(BinarySearch(this.pxList, d => d, pointList[j].X));
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
						this.pyListList[xIndex].Add(new Tuple<double, double>(y1 + (this.pxList[xIndex] - x1) * slope, slope));
					}
				}
			}

			// sort y and slope lists
			for (int xIndex = 0; xIndex < this.pyListList.Count; ++xIndex)
			{
				this.pyListList[xIndex].Sort((t1, t2) => Math.Sign(t1.Item1 == t2.Item1 ? t1.Item2 - t2.Item2 : t1.Item1 - t2.Item1));
			}
		}

		public void Update(List<List<Point3D>> pointListList)
		{
			this.pxList.Clear();
			this.pyListList.Clear();
			if (pointListList != null)
			{
				this.FillStrips(pointListList.Where(x => x.Count >= 3));
			}
		}

		public bool Contains(double x, double y)
		{
			int xIndex = BinarySearch(this.pxList, d => d, x);
			if (xIndex < 0)
			{
				return false;
			}
			double dx = x - this.pxList[xIndex];
			List<Tuple<double, double>> pyList = this.pyListList[xIndex];
			int yIndex = BinarySearch(pyList, t => t.Item1 + dx * t.Item2, y);
			if (yIndex < 0)
			{
				return false;
			}
			if (pyList[yIndex].Item1 + pyList[yIndex].Item2 * dx == y)
			{
				return true;
			}

			return yIndex % 2 == 0;
		}
	}

	public class RegionFilter_Fast: IRegionFilter
	{
		private readonly List<double> pxList = new List<double>();
		private readonly List<List<Tuple<double, double>>> pyListList = new List<List<Tuple<double, double>>>();

		private static int BinarySearch(List<double> valueList, double value)
		{
			int begin = -1;
			int end = valueList.Count;
			int middle;
			while (end - begin > 1)
			{
				middle = (end + begin) >> 1;
				if (valueList[middle] < value)
					begin = middle;
				else
					end = middle;
			}
			return begin;
		}

		private void FillStrips(IEnumerable<List<Point3D>> pointListList)
		{
			// fill sorted x list
			SortedSet<double> sortedSet = new SortedSet<double>();
			foreach (List<Point3D> pointList in pointListList)
			{
				for (int j = 0; j < pointList.Count; ++j)
				{
					sortedSet.Add(pointList[j].X);
				}
			}
			this.pxList.AddRange(sortedSet);
			foreach (double x in this.pxList)
			{
				this.pyListList.Add(new List<Tuple<double, double>>());
			}

			// fill y and slope lists
			foreach (List<Point3D> pointList in pointListList)
			{
				List<int> indexList = new List<int>();
				for (int j = 0; j < pointList.Count; ++j)
				{
					indexList.Add(BinarySearch(this.pxList, pointList[j].X));
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
						this.pyListList[xIndex].Add(new Tuple<double, double>(y1 + (this.pxList[xIndex] - x1) * slope, slope));
					}
				}
			}

			// sort y and slope lists
			for (int xIndex = 0; xIndex < this.pyListList.Count; ++xIndex)
			{
				this.pyListList[xIndex].Sort((t1, t2) => Math.Sign(t1.Item1 == t2.Item1 ? t1.Item2 - t2.Item2 : t1.Item1 - t2.Item1));
			}
		}

		public void Update(List<List<Point3D>> pointListList)
		{
			this.pxList.Clear();
			this.pyListList.Clear();
			if (pointListList != null)
			{
				this.FillStrips(pointListList.Where(x => x.Count >= 3));
			}
		}

		public bool Contains(double x, double y)
		{
			int xIndex = -1;
			int end = this.pxList.Count;
			int middle;
			while (end - xIndex > 1)
			{
				middle = (end + xIndex) >> 1;
				if (this.pxList[middle] < x)
					xIndex = middle;
				else
					end = middle;
			}
			if (xIndex < 0)
			{
				return false;
			}

			double dx = x - this.pxList[xIndex];
			List<Tuple<double, double>> pyList = this.pyListList[xIndex];
			int yIndex = -1;
			end = pyList.Count;
			while (end - yIndex > 1)
			{
				middle = (end + yIndex) >> 1;
				if (pyList[middle].Item1 + dx * pyList[middle].Item2 < y)
					yIndex = middle;
				else
					end = middle;
			}
			if (yIndex < 0)
			{
				return false;
			}
			if (pyList[yIndex].Item1 + dx * pyList[yIndex].Item2 == y)
			{
				return true;
			}

			return yIndex % 2 == 0;
		}
	}
}
