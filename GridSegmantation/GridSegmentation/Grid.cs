using System;

namespace GridSegmentation
{
  public class Grid
  {
    public int iCount { get; private set; }
    public int jCount { get; private set; }
    public int cellsCount { get; private set; }
    public int[] cells { get; private set; }

    public Grid(int iCount = 0, int jCount = 0)
    {
      this.Init(iCount, jCount);
    }

    public void Init(int iCount, int jCount)
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

