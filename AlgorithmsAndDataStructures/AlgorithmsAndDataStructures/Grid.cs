using System;

namespace AlgorithmsAndDataStructures
{
    public class Grid
    {
        private int[] cells;

        public int ICount { get; private set; }
        public int JCount { get; private set; }
        public int CellsCount { get; private set; }
        public int this[int i]
        {
            get => this.cells[i];
            set => this.cells[i] = value;
        }
        public int this[int i, int j]
        {
            get => this.cells[this.Index(i, j)];
            set => this.cells[this.Index(i, j)] = value;
        }

        public Grid(int count = 0, int jCount = 0)
        {
            this.ICount = count;
            this.JCount = jCount;
            this.CellsCount = this.ICount * this.JCount;

            this.cells = new int[this.CellsCount];
        }

        public int Index(int i, int j)
        {
            return j * this.ICount + i;
        }
        public void Index(int index, out int i, out int j)
        {
            i = index % this.ICount;
            j = index / this.ICount;
        }

        public void Clear(int index, int length)
        {
            Array.Clear(this.cells, index, length);
        }

        public Grid Copy()
        {
            var grid = new Grid(this.ICount, this.JCount);
            Array.Copy(this.cells, grid.cells, this.CellsCount);
            return grid;
        }
        public void Copy(Grid grid)
        {
            this.ICount = grid.ICount;
            this.JCount = grid.JCount;
            this.CellsCount = this.ICount * this.JCount;

            this.cells = new int[this.CellsCount];
            Array.Copy(grid.cells, this.cells, this.CellsCount);
        }
    }
}

