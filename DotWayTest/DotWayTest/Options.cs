using System;
using System.Drawing;

namespace DotWayTest
{
    static class Options
    {
        public static readonly Random random = new Random();

        public static Point[] Dots = null;

        public static float MapWidth = 1000;
        public static float MapHeight = 1000;

        public const int DotsRadius = 10;
        public const float BorderSize = 0.1f;

        public static int DotsCount = 2;
        public static bool IsFinish = false;

        public static float CurrentDistance = 0;
    }
}
