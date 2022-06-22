using System;
using NPack;
using UnityEngine;

namespace URandom
{
	public static class RandomDisk
	{
		public static Vector2 Circle(ref MersenneTwister _rand)
		{
			float x = _rand.Next();
			float newMax = (float)Math.PI * 2f;
			float num = SpecialFunctions.ScaleFloatToRange(x, 0f, newMax, 0f, 2.1474836E+09f);
			return new Vector2((float)Math.Cos(num), (float)Math.Sin(num));
		}

		public static Vector2 Circle(ref MersenneTwister _rand, UnityRandom.Normalization n, float t)
		{
			float x = n switch
			{
				UnityRandom.Normalization.STDNORMAL => SpecialFunctions.ScaleFloatToRange((float)NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), t), 0f, 2.1474836E+09f, 0f, 1f), 
				UnityRandom.Normalization.POWERLAW => (float)PowerLaw.Normalize(_rand.NextSingle(includeOne: true), t, 0f, 2.1474836E+09f), 
				_ => _rand.Next(), 
			};
			float newMax = (float)Math.PI * 2f;
			float num = SpecialFunctions.ScaleFloatToRange(x, 0f, newMax, 0f, 2.1474836E+09f);
			return new Vector2((float)Math.Cos(num), (float)Math.Sin(num));
		}

		public static Vector2 Disk(ref MersenneTwister _rand)
		{
			double d = _rand.NextSingle(includeOne: true);
			double num = (double)(_rand.NextSingle(includeOne: false) * 2f) * Math.PI;
			return new Vector2((float)(Math.Sqrt(d) * Math.Cos(num)), (float)(Math.Sqrt(d) * Math.Sin(num)));
		}

		public static Vector2 Disk(ref MersenneTwister _rand, UnityRandom.Normalization n, float temp)
		{
			double d;
			double num;
			switch (n)
			{
			case UnityRandom.Normalization.STDNORMAL:
				d = NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), temp);
				num = NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), temp) * 2.0 * Math.PI;
				break;
			case UnityRandom.Normalization.POWERLAW:
				d = PowerLaw.Normalize(_rand.NextSingle(includeOne: true), temp, 0f, 1f);
				num = PowerLaw.Normalize(_rand.NextSingle(includeOne: true), temp, 0f, 1f) * 2.0 * Math.PI;
				break;
			default:
				d = _rand.NextSingle(includeOne: true);
				num = (double)(_rand.NextSingle(includeOne: false) * 2f) * Math.PI;
				break;
			}
			return new Vector2((float)(Math.Sqrt(d) * Math.Cos(num)), (float)(Math.Sqrt(d) * Math.Sin(num)));
		}
	}
}
