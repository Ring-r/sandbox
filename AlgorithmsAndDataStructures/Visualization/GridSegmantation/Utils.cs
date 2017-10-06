using System;
using System.Collections.Generic;

namespace GridSegmentation
{
	public class Utils
	{
		private static Lazy<Random> lazyRandom = new Lazy<Random>(() => new Random());
		public static Random Random { get { return lazyRandom.Value; } }

		private static IEnumerable<bool> Border(Grid grid, int index, List<int> gridVector)
		{
			int iStart; int jStart; grid.Index(index, out iStart, out jStart);

			var iCurr = iStart; var iv = 0;
			var jCurr = jStart; var jv = 1;

			var isExit = false;
			do
			{
				if (iv == 0)
				{
					gridVector.Add(grid.Index(iCurr, jCurr));
				}

				var iNext = iCurr + iv; var jNext = jCurr + jv;
				var indexNext = grid.Index(iNext, jNext);
				var bNext =
					0 <= iNext && iNext < grid.iCount &&
					0 <= jNext && jNext < grid.jCount &&
					grid[indexNext] > 0;

				var iLeft = iNext - jv; var jLeft = jNext + iv;
				var indexLeft = grid.Index(iLeft, jLeft);
				var bLeft =
					0 <= iLeft && iLeft < grid.iCount &&
					0 <= jLeft && jLeft < grid.jCount &&
					grid[indexLeft] > 0;

				if (bLeft)
				{
					iCurr = iLeft; jCurr = jLeft;
					var ib = iv; iv = -jv; jv = ib;
				}
				else if (bNext)
				{
					iCurr = iNext; jCurr = jNext;
				}
				else
				{
					var ib = iv; iv = jv; jv = -ib;
				}
				yield return true;
			} while (!(iCurr == iStart && iv == 0 && jCurr == jStart && jv == 1));

			if (isExit)
			{
				gridVector.Clear();
			}
			else
			{
				gridVector.Sort();
				var count = gridVector.Count;
				for (var i = 0; i < count; i += 2)
				{
					for (var j = gridVector[i]; j <= gridVector[i + 1]; ++j)
					{
						grid[j] = 0;
					}
				}
			}
		}

		private static int GridCount(List<int> gridVector)
		{
			var gridCount = 0;
			var count = gridVector.Count;
			for (var i = 0; i < count; i += 2)
			{
				gridCount += gridVector[i + 1] - gridVector[i] + 1;
			}
			return gridCount;
		}

		private static int GridClear(Grid grid, List<int> gridVector)
		{
			var gridCount = 0;
			var count = gridVector.Count;
			for (var i = 0; i < count; i += 2)
			{
				for (var j = gridVector[i]; j <= gridVector[i + 1]; j++)
				{
					grid[j] = 0;
				}
			}
			return gridCount;
		}

		private static bool AtBorder(List<int> gridVector, Grid grid)
		{
			var atBorder = false;
			var count = gridVector.Count;
			for (var i = 0; i < count && !atBorder; ++i)
			{
				int index_i, index_j; grid.Index(gridVector[i], out index_i, out index_j);
				atBorder =
					index_i <= 0 || grid.iCount - 1 <= index_i ||
					index_j <= 0 || grid.jCount - 1 <= index_j;
			}
			return atBorder;
		}

		public static IEnumerable<bool> Segmentate(Grid grid, List<List<int>> gridVectors, int minGridCount, bool minGridClear)
		{
			var grid_temp = grid.Copy();

			gridVectors.Clear();

			var vectIndex = -1;
			for (var index = 0; index < grid_temp.cellsCount; ++index)
			{
				if (grid_temp[index] != 0)
				{
					if (vectIndex < 0 || gridVectors[vectIndex].Count > 0)
					{
						gridVectors.Add(new List<int>());
						vectIndex += 1;
					}

					var gridVector = gridVectors[vectIndex];
					foreach (var res in Border(grid_temp, index, gridVector)) ;// yield return res;

					if (GridCount(gridVector) < minGridCount)// && !AtBorder(gridVector, grid))
					{
						if (minGridClear)
						{
							GridClear(grid, gridVector);
						}
						gridVector.Clear();
					}

					yield return true;
				}
			}
		}
	}
}
