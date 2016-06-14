using System;
using System.Collections.Generic;
using System.Drawing;

namespace GridSegmentation
{
  class Utils
  {
    private static Lazy<Random> lazyRandom = new Lazy<Random>(() => new Random());
    public static Random Random { get { return lazyRandom.Value; } }

    private static void Border(Grid grid, int index, List<int> gridVector, Func<bool> afterstep)
    {
      int iStart; int jStart; grid.Index(index, out iStart, out jStart);

      int iCurr = iStart; int iv = 0;
      int jCurr = jStart; int jv = 1;

      bool isExit = false;
      do
      {
        if (iv == 0)
        {
          gridVector.Add(grid.Index(iCurr, jCurr));
        }

        int iNext = iCurr + iv; int jNext = jCurr + jv;
        int indexNext = grid.Index(iNext, jNext);
        bool bNext =
            0 <= iNext && iNext < grid.iCount &&
            0 <= jNext && jNext < grid.jCount &&
            grid[indexNext] > 0;

        int iLeft = iNext - jv; int jLeft = jNext + iv;
        int indexLeft = grid.Index(iLeft, jLeft);
        bool bLeft =
            0 <= iLeft && iLeft < grid.iCount &&
            0 <= jLeft && jLeft < grid.jCount &&
            grid[indexLeft] > 0;

        if (bLeft)
        {
          iCurr = iLeft; jCurr = jLeft;
          int ib = iv; iv = -jv; jv = ib;
        }
        else if (bNext)
        {
          iCurr = iNext; jCurr = jNext;
        }
        else
        {
          int ib = iv; iv = jv; jv = -ib;
        }

        if (afterstep != null && afterstep())
        {
          isExit = true;
          break;
        }

      } while (!(iCurr == iStart && iv == 0 && jCurr == jStart && jv == 1));

      if (isExit)
      {
        gridVector.Clear();
      }
      else
      {
        gridVector.Sort();
        int count = gridVector.Count;
        for (int i = 0; i < count; i += 2)
        {
          for (int j = gridVector[i]; j <= gridVector[i + 1]; ++j)
          {
            grid[j] = 0;
          }
        }
      }
    }

    private static int GridCount(List<int> gridVector)
    {
      int gridCount = 0;
      int count = gridVector.Count;
      for (int i = 0; i < count; i += 2)
      {
        gridCount += gridVector[i + 1] - gridVector[i] + 1;
      }
      return gridCount;
    }

    private static int GridClear(Grid grid, List<int> gridVector)
    {
      int gridCount = 0;
      int count = gridVector.Count;
      for (int i = 0; i < count; i += 2)
      {
        for (int j = gridVector[i]; j <= gridVector[i + 1]; j++)
        {
          grid[j] = 0;
        }
      }
      return gridCount;
    }

    private static bool AtBorder(List<int> gridVector, Grid grid)
    {
      bool atBorder = false;
      int count = gridVector.Count;
      for (int i = 0; i < count && !atBorder; ++i)
      {
        int index_i, index_j; grid.Index(gridVector[i], out index_i, out index_j);
        atBorder =
            index_i <= 0 || grid.iCount - 1 <= index_i ||
            index_j <= 0 || grid.jCount - 1 <= index_j;
      }
      return atBorder;
    }

    public static void Segmentate(Grid grid, List<List<int>> gridVectors, int minGridCount, bool minGridClear, Func<bool> afterstep)
    {
      Grid grid_temp = grid.Copy();

      gridVectors.Clear();

      int vectIndex = -1;
      for (int index = 0; index < grid_temp.cellsCount; ++index)
      {
        if (grid_temp[index] != 0)
        {
          if (vectIndex < 0 || gridVectors[vectIndex].Count > 0)
          {
            gridVectors.Add(new List<int>());
            vectIndex += 1;
          }

          List<int> gridVector = gridVectors[vectIndex];
          Border(grid_temp, index, gridVector, afterstep);

          if (GridCount(gridVector) < minGridCount)// && !AtBorder(gridVector, grid))
          {
            if (minGridClear)
            {
              GridClear(grid, gridVector);
            }
            gridVector.Clear();
          }

          if (afterstep != null && afterstep())
          {
            break;
          }
        }
      }
    }
  }
}
