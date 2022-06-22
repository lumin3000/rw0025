using System;

namespace URandom
{
	public static class SpecialFunctions
	{
		private static double[] cof = new double[6] { 76.18009172947146, -86.50532032941678, 24.01409824083091, -1.231739572450155, 0.001208650973866179, -5.395239384953E-06 };

		public static double gammln(double xx)
		{
			double num;
			double num2 = (num = xx);
			double num3 = num + 5.5;
			num3 -= (num + 0.5) * Math.Log(num3);
			double num4 = 1.000000000190015;
			for (int i = 0; i <= 5; i++)
			{
				num4 += cof[i] / (num2 += 1.0);
			}
			return 0.0 - num3 + Math.Log(2.5066282746310007 * num4 / num);
		}

		public static float ScaleFloatToRange(float x, float newMin, float newMax, float oldMin, float oldMax)
		{
			return x / ((oldMax - oldMin) / (newMax - newMin)) + newMin;
		}
	}
}
