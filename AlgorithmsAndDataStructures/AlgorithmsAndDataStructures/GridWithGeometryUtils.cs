using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace AlgorithmsAndDataStructures
{
    public static class GridWithGeometryUtils
    {
        public static void Border(this GridWithGeometry<bool> grid, List<List<Point3D>> region)
        {
            foreach (var part in region)
            {
                for (var i = 0; i < part.Count - 1; ++i)
                {
                    grid.Border(part[i], part[i + 1]);
                }
                grid.Border(part[0], part[part.Count - 1]);
            }
        }

        private static void Border(this GridWithGeometry<bool> grid, Point3D point0, Point3D point1)
        {
            if (point0.Y > point1.Y)
            {
                Utils.Swap(ref point0, ref point1);
            }

            var ix0 = grid.GetCellIndexByX(point0.X);
            var iy0 = grid.GetCellIndexByY(point0.Y);
            var ix1 = grid.GetCellIndexByX(point1.X);
            var iy1 = grid.GetCellIndexByY(point1.Y);

            if (iy0 == iy1)
            {
                grid.SetValue(true, ix0, ix1, iy0, iy1);
                return;
            }

            var k = (point1.X - point0.X) / (point1.Y - point0.Y); // x = f(y) = x0 + k * (y - y0)

            var ixPrev = ix0;
            int ixNext;
            for (var iy = iy0 + 1; iy <= iy1; ++iy)
            {
                var y = iy * grid.JStepSize;
                var x = point0.X + k * (y - point0.Y);

                ixNext = grid.GetCellIndexByX(x);
                grid.SetValue(true, ixPrev, ixNext, iy - 1, iy - 1);

                ixPrev = ixNext;
            }
            ixNext = ix1;
            grid.SetValue(true, ixPrev, ixNext, iy1, iy1);
        }
    }
}
