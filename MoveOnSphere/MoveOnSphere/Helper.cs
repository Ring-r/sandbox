using System;

namespace MoveOnSphere
{
	public static class Helper
	{
		public static readonly Random Random = new Random();

		public static float RandomFloat(float min, float max)
		{
			return (float)((max - min) * Random.NextDouble() + min);
		}

		public static int RandomSign ()
		{
			int s = Random.Next (2);
			if (s == 0) {
				s = -1;
			}
			return s;
		}
	}
}

