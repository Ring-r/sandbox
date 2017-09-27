namespace AlgorithmsAndDataStructures
{
    public class GridWithGeometry<T> : Grid<T>
    {
        public double IStepSize { get; set; }
        public double JStepSize { get; set; }

        public GridWithGeometry(int iCount, int jCount)
            : base(iCount, jCount)
        {
        }

        public int GetCellIndexByX(double x)
        {
            return GetIndex(x, this.IStepSize);
        }

        public int GetCellIndexByY(double y)
        {
            return GetIndex(y, this.JStepSize);
        }

        private static int GetIndex(double value, double step)
        {
            return (int)(value / step);
        }
    }
}
