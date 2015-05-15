using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace RegionFilter
{
	public class RegionFilter: IRegionFilter
	{
		private double[] pxArray = new double[0];
		private double[][][] pyArrays = new double[0][][];

		private double[] GetSortedXArray(List<List<Point3D>> pointListList)
		{
			SortedSet<double> sortedSet = new SortedSet<double>();
			foreach (List<Point3D> pointList in pointListList)
			{
				foreach (Point3D point in pointList)
				{
					sortedSet.Add(point.X);
				}
			}
			return sortedSet.ToArray();
		}

		private void FillStrips(List<List<Point3D>> pointListList)
		{
			this.pxArray = this.GetSortedXArray(pointListList);
			this.pyArrays = new double[this.pxArray.Length][][];

			List<double[]>[] pyLists = new List<double[]>[this.pxArray.Length];
			for (int i = 0; i < pyLists.Length; ++i)
			{
				pyLists[i] = new List<double[]>();
			}

			foreach (List<Point3D> pointList in pointListList)
			{
				List<int> indexList = new List<int>(pointList.Count);
				for (int j = 0; j < pointList.Count; ++j)
				{
					indexList.Add(Array.BinarySearch(this.pxArray, pointList[j].X) - 1);
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
						pyLists[xIndex].Add(new double[2] { y1 + (this.pxArray[xIndex] - x1) * slope, slope });
					}
				}
			}

			for (int xIndex = 0; xIndex < pyLists.Length; ++xIndex)
			{
				pyLists[xIndex].Sort((t1, t2) => Math.Sign(t1[0] == t2[0] ? t1[1] - t2[1] : t1[0] - t2[0]));
				this.pyArrays[xIndex] = pyLists[xIndex].ToArray();
			}
		}

		public void Update(List<Point3D> pointListList)
		{
			this.Update(pointListList.AsEnumerable().ToList());
		}

		public void Update(List<List<Point3D>> pointListList)
		{
			this.pxArray = new double[0];
			this.pyArrays = new double[0][][];
			this.FillStrips(pointListList);
		}

		public bool Contains(double x, double y)
		{
			int xBegin = -1;
			int xEnd = this.pxArray.Length;
			int xMiddle;
			while (xEnd - xBegin > 1)
			{
				xMiddle = (xEnd + xBegin) >> 1;
				if (this.pxArray[xMiddle] < x)
					xBegin = xMiddle;
				else
					xEnd = xMiddle;
			}
			if (xBegin < 0)
			{
				return false;
			}

			double dx = x - this.pxArray[xBegin];
			double[][] pyList = this.pyArrays[xBegin];
			int yBegin = -1;
			int yEnd = pyList.Length;
			int yMiddle;
			while (yEnd - yBegin > 1)
			{
				yMiddle = (yEnd + yBegin) >> 1;
				if (pyList[yMiddle][0] + dx * pyList[yMiddle][1] < y)
					yBegin = yMiddle;
				else
					yEnd = yMiddle;
			}
			if (yBegin < 0)
			{
				return false;
			}
			if (pyList[yBegin][0] + dx * pyList[yBegin][1] == y)
			{
				return true;
			}

			return yBegin % 2 == 0;
		}
	}
}
