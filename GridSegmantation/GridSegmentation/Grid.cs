using System;

namespace GridSegmentation
{
	public class Grid
	{
        public int iCount;
        public int jCount;
		public int cellsCount;
		public int[] cells;

		public void Init (int iCount, int jCount)
		{
            this.iCount = 100;
            this.jCount = 100;
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
	}
}

