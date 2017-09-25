using System.Collections.Generic;

namespace AlgorithmsAndDataStructures
{
    public class GridSegmentation
    {
        public static List<List<int>> Segmentate(Grid grid)
        {
            var resultGridVectors = new List<List<int>>();

            var gridTemp = grid.Copy();

            for (var index = 0; index < gridTemp.CellsCount; ++index)
            {
                if (gridTemp[index] == 0)
                {
                    continue;
                }

                var gridVector = Border(gridTemp, index);
                GridClear(gridTemp, gridVector);
                resultGridVectors.Add(gridVector);
            }

            return resultGridVectors;
        }

        private static List<int> Border(Grid grid, int startIndex)
        {
            var resultGridVector = new List<int>();

            grid.Index(startIndex, out var iStart, out var jStart);

            var iCurr = iStart;
            var iv = 0;
            var jCurr = jStart;
            var jv = 1;

            do
            {
                if (iv == 0)
                {
                    resultGridVector.Add(grid.Index(iCurr, jCurr));
                }

                var iNext = iCurr + iv;
                var jNext = jCurr + jv;
                var indexNext = grid.Index(iNext, jNext);
                var bNext =
                    0 <= iNext && iNext < grid.ICount &&
                    0 <= jNext && jNext < grid.JCount &&
                    grid[indexNext] > 0;

                var iLeft = iNext - jv;
                var jLeft = jNext + iv;
                var indexLeft = grid.Index(iLeft, jLeft);
                var bLeft =
                    0 <= iLeft && iLeft < grid.ICount &&
                    0 <= jLeft && jLeft < grid.JCount &&
                    grid[indexLeft] > 0;

                if (bLeft)
                {
                    iCurr = iLeft;
                    jCurr = jLeft;
                    var ib = iv;
                    iv = -jv;
                    jv = ib;
                }
                else if (bNext)
                {
                    iCurr = iNext; jCurr = jNext;
                }
                else
                {
                    var ib = iv;
                    iv = jv;
                    jv = -ib;
                }
            } while (!(iCurr == iStart && iv == 0 && jCurr == jStart && jv == 1));

            resultGridVector.Sort();

            return resultGridVector;
        }

        private static void GridClear(Grid grid, IReadOnlyList<int> gridVector)
        {
            for (var i = 0; i < gridVector.Count; i += 2)
            {
                grid.Clear(gridVector[i], gridVector[i + 1] - gridVector[i] + 1);
            }
        }

        private static int GridCount(IReadOnlyList<int> gridVector)
        {
            var gridCount = 0;
            for (var i = 0; i < gridVector.Count; i += 2)
            {
                gridCount += gridVector[i + 1] - gridVector[i] + 1;
            }
            return gridCount;
        }

        private static bool AtBorder(Grid grid, IReadOnlyList<int> gridVector)
        {
            var atBorder = false;
            for (var i = 0; i < gridVector.Count && !atBorder; ++i)
            {
                grid.Index(gridVector[i], out var iIndex, out var jIndex);
                atBorder =
                    iIndex <= 0 || grid.ICount - 1 <= iIndex ||
                    jIndex <= 0 || grid.JCount - 1 <= jIndex;
            }
            return atBorder;
        }
    }
}
