using NPack;
using UnityEngine;

namespace URandom
{
	public static class RandomSquare
	{
		public static Vector2 Area(ref MersenneTwister _rand)
		{
			return new Vector2(2f * _rand.NextSingle(includeOne: true) - 1f, 2f * _rand.NextSingle(includeOne: true) - 1f);
		}

		public static Vector2 Area(ref MersenneTwister _rand, UnityRandom.Normalization n, float t)
		{
			float num = 0f;
			float num2;
			switch (n)
			{
			case UnityRandom.Normalization.STDNORMAL:
				num2 = (float)NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), t);
				num = (float)NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), t);
				break;
			case UnityRandom.Normalization.POWERLAW:
				num2 = (float)PowerLaw.Normalize(_rand.NextSingle(includeOne: true), t, 0f, 1f);
				num = (float)PowerLaw.Normalize(_rand.NextSingle(includeOne: true), t, 0f, 1f);
				break;
			default:
				num2 = _rand.NextSingle(includeOne: true);
				num = _rand.NextSingle(includeOne: true);
				break;
			}
			return new Vector2(2f * num2 - 1f, 2f * num - 1f);
		}
	}
}
