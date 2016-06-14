using System;

namespace GridSegmentation
{
  public class Grid
  {
    private int[] cells;

    public int iCount { get; private set; }
    public int jCount { get; private set; }
    public int cellsCount { get; private set; }
    public int this[int i] { get { return this.cells[i]; } set { this.cells[i] = value; } }

    public Grid(int iCount = 0, int jCount = 0)
    {
      this.iCount = iCount;
      this.jCount = jCount;
      this.cellsCount = this.iCount * this.jCount;

      this.cells = new int[this.cellsCount];
    }

    public int Index(int i, int j)
    {
      return j * this.iCount + i;
    }
    public void Index(int index, out int i, out int j)
    {
      i = index % this.iCount;
      j = index / this.iCount;
    }

    public Grid Copy()
    {
      Grid grid = new Grid(this.iCount, this.jCount);
      Array.Copy(this.cells, grid.cells, this.cellsCount);
      return grid;
    }
    public void Copy(Grid grid)
    {
      this.iCount = grid.iCount;
      this.jCount = grid.jCount;
      this.cellsCount = this.iCount * this.jCount;

      this.cells = new int[this.cellsCount];
      Array.Copy(grid.cells, this.cells, this.cellsCount);
    }

    public void InitCells(int filledCountPercents, int value = 1)
    {
      Array.Clear(this.cells, 0, this.cellsCount);

      int filledCount = filledCountPercents * this.cellsCount / 100;
      for (int i = 0; i < filledCount; ++i)
      {
        this.cells[this.Index(Utils.Random.Next(this.iCount), Utils.Random.Next(this.jCount))] = value;
      }
    }
  }
}

