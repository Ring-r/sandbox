using System;

namespace Prototype
{
    public static class Helper
    {
        public static readonly Random Random = new Random();
        public static float RandomFloat(float min, float max)
        {
            return (float)(min + (max - min) * Random.NextDouble());
        }

        public static float ExtDistance(Entity entity, Entity entityNext)
        {
            float x = entityNext.X - entity.X;
            float y = entityNext.Y - entity.Y;
            return (float)Math.Sqrt(x * x + y * y) - entityNext.R - entity.R;
        }
    }
}

