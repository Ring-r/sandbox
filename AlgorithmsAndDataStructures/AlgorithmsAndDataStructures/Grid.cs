using System;

namespace AlgorithmsAndDataStructures
{
    public class Grid<T>
    {
        private T[] cells;

        public int ICount { get; private set; }
        public int JCount { get; private set; }
        public int CellsCount { get; private set; }
        public T this[int i]
        {
            get => this.cells[i];
            set => this.cells[i] = value;
        }
        public T this[int i, int j]
        {
            get => this.cells[this.Index(i, j)];
            set => this.cells[this.Index(i, j)] = value;
        }

        public Grid(int iCount, int jCount)
        {
            this.ReInitialize(iCount, jCount);
        }

        public void ReInitialize(int iCount, int jCount)
        {
            this.ICount = iCount;
            this.JCount = jCount;
            this.cells = new T[this.ICount * this.JCount];
        }

        public void SetValue(T value, int i0, int i1, int j0, int j1)
        {
            if (i0 > i1)
            {
                Utils.Swap(ref i0, ref i1);
            }
            if (j0 > j1)
            {
                Utils.Swap(ref j0, ref j1);
            }

            for (var i = i0; i <= i1; ++i)
            {
                for (var j = j0; j <= j1; ++j)
                {
                    this[i, j] = value;
                }
            }
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

        public Grid<T> Copy()
        {
            var grid = new Grid<T>(this.ICount, this.JCount);
            Array.Copy(this.cells, grid.cells, this.CellsCount);
            return grid;
        }
        public void Copy(Grid<T> grid)
        {
            this.ICount = grid.ICount;
            this.JCount = grid.JCount;
            this.CellsCount = this.ICount * this.JCount;

            this.cells = new T[this.CellsCount];
            Array.Copy(grid.cells, this.cells, this.CellsCount);
        }
    }
}

