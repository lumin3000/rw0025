using System;
using NPack;

namespace URandom
{
	public static class GammaDistribution
	{
		public static float Normalize(ref MersenneTwister _rand, int ia)
		{
			if (ia < 1)
			{
				throw new ArgumentException("Error in Gamma Distribution. Argument ia should be an integer > 1");
			}
			float num;
			if (ia < 6)
			{
				num = 1f;
				for (int i = 1; i <= ia; i++)
				{
					num *= _rand.NextSingle(includeOne: true);
				}
				num = (float)(0.0 - Math.Log(num));
			}
			else
			{
				while (true)
				{
					float num2 = _rand.NextSingle(includeOne: true);
					float num3 = 2f * _rand.NextSingle(includeOne: true) - 1f;
					if (num2 * num2 + num3 * num3 > 1f)
					{
						continue;
					}
					float num4 = num3 / num2;
					float num5 = ia - 1;
					float num6 = (float)Math.Sqrt(2.0 * (double)num5 + 1.0);
					num = num6 * num4 + num5;
					if (!(num <= 0f))
					{
						float num7 = (float)((double)(1f + num4 * num4) * Math.Exp((double)num5 * Math.Log(num / num5) - (double)(num6 * num4)));
						if (!(_rand.NextSingle(includeOne: true) > num7))
						{
							break;
						}
					}
				}
			}
			return num;
		}
	}
}
