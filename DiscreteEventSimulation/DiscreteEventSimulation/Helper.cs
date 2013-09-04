using System;

namespace DiscreteEventSimulation
{
    public static class Helper
    {
        public static readonly Random Random = new Random();
        public static float RandomFloat(float min, float max)
        {
            return (float)(min + (max - min) * Random.NextDouble());
        }
    }
}

