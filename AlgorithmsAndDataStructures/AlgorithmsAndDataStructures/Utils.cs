namespace AlgorithmsAndDataStructures
{
    public static class Utils
    {
        public static void Swap<T>(ref T x, ref T y)
        {
            var t = y;
            y = x;
            x = t;
        }
    }
}
