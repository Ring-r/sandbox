using System;
using System.Drawing;

namespace DotWayTest
{
    static class Options
    {
        public enum StateEnum { BeforChoose, Choose, Wait };
        public static StateEnum State = StateEnum.BeforChoose;

        public static readonly Random random = new Random();

        public static int DotsCount = 2;
        public const int DotsRadius = 10;

        public static bool IsFinish = false;

        public static float CurrentDistance = 0;
    }
}
