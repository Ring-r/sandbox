using System;
using System.Collections.Generic;
using System.Drawing;

namespace Prototypes.Maps.StripsAndRooms
{
  public class Map
  {
    public class Settings
    {
      public UInt16 LinesCount { get; set; }
      public UInt16 LineSize { get; set; }
      public UInt16 ICount { get; set; }
      public UInt16 JCount { get { return (UInt16)(LinesCount * LineSize + (LinesCount + 1)); } }
      public Byte PercentsOfMoving { get; set; }

      private static Lazy<Settings> _default = new Lazy<Settings>(() => new Settings()
      {
        LinesCount = 4,
        LineSize = 4,
        ICount = 100,
        PercentsOfMoving = 5,
      });
      public static Settings Default { get { return _default.Value; } }
    }
    public class Style
    {
      public Dictionary<Int32, Brush> CellBrushes { get; set; }
      public UInt16 CellSize { get; set; }
      public Brush ErrorBrush { get; set; }

      private static Lazy<Style> _default = new Lazy<Style>(() => new Style()
      {
        CellBrushes = new Dictionary<Int32, Brush>()
        {
          {0, Brushes.Gray},
          {1, Brushes.Orange},
          {-1, Brushes.Yellow},
        },
        CellSize = 16,
        ErrorBrush = Brushes.Red,
      });
      public static Style Default { get { return _default.Value; } }
    }

    public Int32[,] cells;

    public void Init(Settings settings)
    {
      if (this.cells == null || this.cells.GetLength(0) != settings.ICount || this.cells.GetLength(1) != settings.JCount)
      {
        this.cells = new Int32[settings.ICount, settings.JCount];
      }
      FillMap(this.cells, settings);
    }

    public void Draw(Graphics graphics, Style style)
    {
      for (int i = 0; i < this.cells.GetLength(0); i++)
      {
        for (int j = 0; j < this.cells.GetLength(1); j++)
        {
          Brush brush = style.CellBrushes.ContainsKey(this.cells[i, j]) ? style.CellBrushes[this.cells[i, j]] : style.ErrorBrush;
          graphics.FillRectangle(brush, i * style.CellSize, j * style.CellSize, style.CellSize, style.CellSize);
        }
      }
    }

    private static void FillMap(Int32[,] cells, Settings settings)
    {
      Array.Clear(cells, 0, cells.Length);

      for (int i = 0; i < cells.GetLength(0); i++)
      {
        cells[i, 0] = 1;
        cells[i, cells.GetLength(1) - 1] = 1;
      }
      for (int j = 0; j < cells.GetLength(1); j++)
      {
        cells[0, j] = 1;
        cells[cells.GetLength(0) - 1, j] = 1;
      }

      for (int i = 0; i < cells.GetLength(0); i++)
      {
        for (int j = 0; j < cells.GetLength(1); j += settings.LineSize + 1)
        {
          cells[i, j] = 1;
        }
      }

      int cellsCountMax = settings.ICount / 3;
      CreateCells(cells, settings, cellsCountMax, true);

      WearOut(cells, settings);
    }
    private static void CreateCells(Int32[,] cells, Settings settings, int maxLineCellsCount, bool isDoor)
    {
      for (int j = 1; j < cells.GetLength(1); j += settings.LineSize + 1)
      {
        int lineCellsCount = Program.Random.Next(maxLineCellsCount);
        for (int k = 0; k < lineCellsCount; k++)
        {
          int i = Program.Random.Next(settings.ICount);
          if (cells[i, j] == 0)
          {
            int wallSize = isDoor ? Program.Random.Next(settings.LineSize) : settings.LineSize;
            for (int ij = 0; ij < wallSize; ij++)
            {
              cells[i, j + ij] = 1;
              cells[settings.ICount - i, j + ij] = 1;
            }
          }
        }
      }
    }
    private static void WearOut(Int32[,] map, Settings settings)
    {
      for (int i = 0; i < map.GetLength(0); i++)
      {
        for (int j = 0; j < map.GetLength(1); j++)
        {
          if (map[i, j] == 1 && Program.Random.Next(100) < settings.PercentsOfMoving)
          {
            int k = Program.Random.Next(settings.LineSize);
            map[i, j] = 0;
            map[i, Math.Min(j + k, map.GetLength(1) - 1)] = -k;
          }
        }
      }

      for (int i = 0; i < map.GetLength(0); i++)
      {
        for (int j = 0; j < map.GetLength(1); j++)
        {
          map[i, j] = map[i, j] != 0 ? 1 : 0;
        }
      }
    }
  }
}
