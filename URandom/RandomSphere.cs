using System;
using NPack;
using UnityEngine;

namespace URandom
{
	public static class RandomSphere
	{
		public static Vector3 Surface(ref MersenneTwister _rand)
		{
			Vector3 pos = PickCubePoints(ref _rand);
			while (IsNotOnSurface(pos))
			{
				pos = PickCubePoints(ref _rand);
			}
			return Normalize(pos);
		}

		public static Vector3 Volume(ref MersenneTwister _rand)
		{
			Vector3 vector = PickCubePoints(ref _rand);
			while (isNotInsideSphere(vector))
			{
				vector = PickCubePoints(ref _rand);
			}
			return vector;
		}

		private static Vector3 PickCubePoints(ref MersenneTwister _rand)
		{
			float x = SpecialFunctions.ScaleFloatToRange(_rand.NextSingle(includeOne: true), -1f, 1f, 0f, 1f);
			float y = SpecialFunctions.ScaleFloatToRange(_rand.NextSingle(includeOne: true), -1f, 1f, 0f, 1f);
			float z = SpecialFunctions.ScaleFloatToRange(_rand.NextSingle(includeOne: true), -1f, 1f, 0f, 1f);
			return new Vector3(x, y, z);
		}

		private static bool isNotInsideSphere(Vector3 pos)
		{
			return pos.x * pos.x + pos.y * pos.y + pos.z * pos.z > 1f;
		}

		private static bool IsNotOnSurface(Vector3 pos)
		{
			return pos.x * pos.x + pos.y * pos.y + pos.z * pos.z > 1f;
		}

		private static Vector3 Normalize(Vector3 pos)
		{
			float num = (float)Math.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
			return new Vector3(pos.x / num, pos.y / num, pos.z / num);
		}

		public static Vector3 GetPointOnCap(float spotAngle, ref MersenneTwister _rand)
		{
			float f = SpecialFunctions.ScaleFloatToRange(_rand.NextSingle(includeOne: true), 0f, (float)Math.PI * 2f, 0f, 1f);
			float f2 = SpecialFunctions.ScaleFloatToRange(_rand.NextSingle(includeOne: true), 0f, spotAngle * ((float)Math.PI / 180f), 0f, 1f);
			Vector3 result = new Vector3(Mathf.Sin(f), Mathf.Cos(f), 0f);
			result *= Mathf.Sin(f2);
			result.z = Mathf.Cos(f2);
			return result;
		}

		public static Vector3 GetPointOnCap(float spotAngle, ref MersenneTwister _rand, Quaternion orientation)
		{
			return orientation * GetPointOnCap(spotAngle, ref _rand);
		}

		public static Vector3 GetPointOnCap(float spotAngle, ref MersenneTwister _rand, Transform relativeTo, float radius)
		{
			return relativeTo.TransformPoint(GetPointOnCap(spotAngle, ref _rand) * radius);
		}

		public static Vector3 GetPointOnRing(float innerSpotAngle, float outerSpotAngle, ref MersenneTwister _rand)
		{
			float f = SpecialFunctions.ScaleFloatToRange(_rand.NextSingle(includeOne: true), 0f, (float)Math.PI * 2f, 0f, 1f);
			float f2 = SpecialFunctions.ScaleFloatToRange(_rand.NextSingle(includeOne: true), innerSpotAngle, outerSpotAngle, 0f, 1f) * ((float)Math.PI / 180f);
			Vector3 result = new Vector3(Mathf.Sin(f), Mathf.Cos(f), 0f);
			result *= Mathf.Sin(f2);
			result.z = Mathf.Cos(f2);
			return result;
		}

		public static Vector3 GetPointOnRing(float innerSpotAngle, float outerSpotAngle, ref MersenneTwister _rand, Quaternion orientation)
		{
			return orientation * GetPointOnRing(innerSpotAngle, outerSpotAngle, ref _rand);
		}

		public static Vector3 GetPointOnRing(float innerSpotAngle, float outerSpotAngle, ref MersenneTwister _rand, Transform relativeTo, float radius)
		{
			return relativeTo.TransformPoint(GetPointOnRing(innerSpotAngle, outerSpotAngle, ref _rand) * radius);
		}
	}
}
