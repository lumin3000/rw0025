using System;

namespace URandom
{
	public static class PowerLaw
	{
		public static double Normalize(float x, float t, float min, float max)
		{
			return Math.Pow((Math.Pow(max, t + 1f) - Math.Pow(min, t + 1f)) * (double)x + Math.Pow(min, t + 1f), 1f / (t + 1f));
		}
	}
}
