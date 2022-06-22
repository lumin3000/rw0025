using System;

namespace URandom
{
	public static class ExponentialDistribution
	{
		public static float Normalize(float randx, float lambda)
		{
			return Convert.ToSingle(Math.Log(1f - randx) / (double)(0f - lambda));
		}
	}
}
