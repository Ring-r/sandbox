using System;
using System.Collections.Generic;
using System.Drawing;

namespace GridSegmentation
{
  public class GridView
  {
    private readonly int cellSize;
    private readonly int filledAlpha;
    private readonly Brush filledBrush;
    private readonly Color borderColor;

    public GridView(int cellSize, Color borderColor, Color filledColor, int gridVectorAlpha)
    {
      this.cellSize = cellSize;
      this.borderColor = borderColor;
      this.filledBrush = new SolidBrush(filledColor);
      this.filledAlpha = gridVectorAlpha;
    }

    public void DrawGrid(Graphics g, Grid grid)
    {
      for (var i = 0; i < grid.iCount; ++i)
      {
        for (var j = 0; j < grid.jCount; ++j)
        {
          if (grid[grid.Index(i, j)] > 0)
          {
            g.FillRectangle(this.filledBrush, i * this.cellSize, j * this.cellSize, this.cellSize, this.cellSize);
          }
        }
      }
    }

    public void DrawGridVectors(Graphics g, Grid grid, List<List<int>> gridVectors)
    {
      if (gridVectors.Count == 0)
      {
        return;
      }

      var random = new Random(0);
      for (var i = 0; i < gridVectors.Count - 1; ++i)
      {
        DrawGridVector(g, grid, gridVectors[i], ColorFrom(random, this.filledAlpha));
      }
      DrawGridVectorBorders(g, grid, gridVectors[gridVectors.Count - 1], this.borderColor);
    }

    private static Color ColorFrom(Random random, int alpha)
    {
      return Color.FromArgb(alpha, random.Next(255), random.Next(255), random.Next(255));
    }

    private void DrawGridVector(Graphics g, Grid grid, List<int> gridVector, Color color)
    {
      Brush brush = new SolidBrush(color);
      var count = gridVector.Count;
      for (var i = 0; i < count; i += 2)
      {
        for (var index = gridVector[i]; index <= gridVector[i + 1]; ++index)
        {
          int index_i, index_j; grid.Index(index, out index_i, out index_j);
          g.FillRectangle(brush, index_i * this.cellSize, index_j * this.cellSize, this.cellSize, this.cellSize);
        }
      }
    }
    private void DrawGridVectorBorders(Graphics g, Grid grid, List<int> gridVector, Color color)
    {
      Brush brush = new SolidBrush(color);
      var count = gridVector.Count;
      for (var i = 0; i < count; ++i)
      {
        int index_i, index_j; grid.Index(gridVector[i], out index_i, out index_j);
        g.FillRectangle(brush, index_i * this.cellSize, index_j * this.cellSize, this.cellSize, this.cellSize);
      }
    }
  }
}
