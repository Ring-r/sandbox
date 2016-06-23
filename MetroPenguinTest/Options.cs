using System;

namespace MetroPenguinTest
{
	static class Options
	{
		private readonly static Lazy<Random> random = new Lazy<Random>(() => new Random());
		public static Random Random { get { return random.Value; } }
		public static float RandomFloat(float min, float max)
		{
			return min + (max - min) * (float)Random.NextDouble();
		}

		public static readonly int startCount = 50;

		public static readonly int minR = 20;
		public static readonly int maxR = 30;
		public static readonly float minS = 50;
		public static readonly float maxS = 101;

		public static readonly int needFPS = 60;

		public static readonly float eps = 1e-5f;

		public static readonly int pointR = 2;
	}
}
