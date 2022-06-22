using NPack;
using UnityEngine;

namespace URandom
{
	public static class RandomCube
	{
		public static Vector3 Surface(ref MersenneTwister _rand)
		{
			Vector3 pointOnCubeSurface = GetPointOnCubeSurface(_rand.NextSingle(includeOne: true), _rand.NextSingle(includeOne: true), _rand.Next(5));
			return new Vector3(2f * pointOnCubeSurface.x - 1f, 2f * pointOnCubeSurface.y - 1f, 2f * pointOnCubeSurface.z - 1f);
		}

		public static Vector3 Surface(ref MersenneTwister _rand, UnityRandom.Normalization n, float t)
		{
			Vector3 vector = default(Vector3);
			vector = n switch
			{
				UnityRandom.Normalization.STDNORMAL => GetPointOnCubeSurface((float)NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), t), (float)NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), t), _rand.Next(5)), 
				UnityRandom.Normalization.POWERLAW => GetPointOnCubeSurface((float)PowerLaw.Normalize(_rand.NextSingle(includeOne: true), t, 0f, 1f), (float)PowerLaw.Normalize(_rand.NextSingle(includeOne: true), t, 0f, 1f), _rand.Next(5)), 
				_ => GetPointOnCubeSurface(_rand.NextSingle(includeOne: true), _rand.NextSingle(includeOne: true), _rand.Next(5)), 
			};
			return new Vector3(2f * vector.x - 1f, 2f * vector.y - 1f, 2f * vector.z - 1f);
		}

		private static Vector3 GetPointOnCubeSurface(float xx, float yy, int side)
		{
			float z;
			float x;
			float y;
			switch (side)
			{
			case 0:
				z = 0f;
				x = xx;
				y = yy;
				break;
			case 1:
				z = 1f;
				x = xx;
				y = yy;
				break;
			case 2:
				z = xx;
				x = yy;
				y = 0f;
				break;
			case 3:
				z = xx;
				x = yy;
				y = 1f;
				break;
			case 4:
				z = xx;
				y = yy;
				x = 0f;
				break;
			case 5:
				z = xx;
				y = yy;
				x = 1f;
				break;
			default:
				x = 0f;
				y = 0f;
				z = 0f;
				break;
			}
			return new Vector3(x, y, z);
		}

		public static Vector3 Volume(ref MersenneTwister _rand)
		{
			Vector3 vector = new Vector3(_rand.NextSingle(includeOne: true), _rand.NextSingle(includeOne: true), _rand.NextSingle(includeOne: true));
			return new Vector3(2f * vector.x - 1f, 2f * vector.y - 1f, 2f * vector.z - 1f);
		}

		public static Vector3 Volume(ref MersenneTwister _rand, UnityRandom.Normalization n, float t)
		{
			float num;
			float num2 = (num = 0f);
			float num3;
			switch (n)
			{
			case UnityRandom.Normalization.STDNORMAL:
				num3 = (float)NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), t);
				num2 = (float)NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), t);
				num = (float)NormalDistribution.Normalize(_rand.NextSingle(includeOne: true), t);
				break;
			case UnityRandom.Normalization.POWERLAW:
				num3 = (float)PowerLaw.Normalize(_rand.NextSingle(includeOne: true), t, 0f, 1f);
				num2 = (float)PowerLaw.Normalize(_rand.NextSingle(includeOne: true), t, 0f, 1f);
				num = (float)PowerLaw.Normalize(_rand.NextSingle(includeOne: true), t, 0f, 1f);
				break;
			default:
				num3 = _rand.NextSingle(includeOne: true);
				num2 = _rand.NextSingle(includeOne: true);
				num = _rand.NextSingle(includeOne: true);
				break;
			}
			return new Vector3(2f * num3 - 1f, 2f * num2 - 1f, 2f * num - 1f);
		}
	}
}
