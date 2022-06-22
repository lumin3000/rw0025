using System;
using NPack;

namespace URandom
{
	public static class PoissonDistribution
	{
		public static float Normalize(ref MersenneTwister _rand, float xm)
		{
			double num3;
			double num2;
			double num;
			double num4 = (num3 = (num2 = (num = -1.0)));
			double num5;
			if (xm < 12f)
			{
				if ((double)xm != num)
				{
					num = xm;
					num2 = Math.Exp(0f - xm);
				}
				num5 = -1.0;
				double num6 = 1.0;
				do
				{
					num5 += 1.0;
					num6 *= (double)_rand.NextSingle(includeOne: true);
				}
				while (num6 > num2);
			}
			else
			{
				if ((double)xm != num)
				{
					num = xm;
					num4 = Math.Sqrt(2.0 * (double)xm);
					num3 = Math.Log(xm);
					num2 = (double)xm * num3 - SpecialFunctions.gammln(xm + 1f);
				}
				while (true)
				{
					double num7 = Math.Tan(Math.PI * (double)_rand.NextSingle(includeOne: true));
					num5 = num4 * num7 + (double)xm;
					if (!(num5 < 0.0))
					{
						num5 = Math.Floor(num5);
						double num6 = 0.9 * (1.0 + num7 * num7) * Math.Exp(num5 * num3 - SpecialFunctions.gammln(num5 + 1.0) - num2);
						if (!((double)_rand.NextSingle(includeOne: true) > num6))
						{
							break;
						}
					}
				}
			}
			return (float)num5;
		}
	}
}
