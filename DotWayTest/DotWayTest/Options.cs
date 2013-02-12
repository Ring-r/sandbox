using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotWayTest
{
    static class Options
    {
        public const int DotsRadius = 10;
        public const float BorderSize = 0.1f;

        public static int DotsCount = 2;
        public static bool IsFinish = false;

        public static float opponentDistance = 0;
        public static readonly List<int> DotsStackOpponent = new List<int>();

        public static float CurrentDistance = 0;
    }
}
