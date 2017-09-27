using System.Drawing;

namespace AlgorithmsAndDataStructures
{
    public class GridWithGeometryViewer
    {
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }

        public void Draw(GridWithGeometry<bool> grid, Graphics graphics, Size clientSize)
        {
            for (var iy = 0; iy < grid.JCount; ++iy)
            {
                var y = iy * grid.JStepSize;
                graphics.DrawLine(this.Pen, 0f, (float)y, clientSize.Width, (float)y);
            }
            for (var ix = 0; ix < grid.ICount; ++ix)
            {
                var x = ix * grid.IStepSize;
                graphics.DrawLine(this.Pen, (float)x, 0, (float)x, clientSize.Height);
            }

            for (var ix = 0; ix < grid.ICount; ++ix)
            {
                var x = ix * grid.IStepSize;
                for (var iy = 0; iy < grid.JCount; ++iy)
                {
                    var y = iy * grid.JStepSize;
                    if (grid[ix, iy])
                    {
                        graphics.FillRectangle(this.Brush, (float)x, (float)y, (float)(grid.IStepSize), (float)(grid.JStepSize));
                    }
                }
            }
        }

    }
}
