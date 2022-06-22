using System;
using UnityEngine;

public static class GenGeo
{
	public static float AngleDifferenceBetween(float A, float B)
	{
		float num = A + 360f;
		float num2 = B + 360f;
		float num3 = 9999f;
		float num4 = 0f;
		num4 = A - B;
		if (num4 < 0f)
		{
			num4 *= -1f;
		}
		if (num4 < num3)
		{
			num3 = num4;
		}
		num4 = num - B;
		if (num4 < 0f)
		{
			num4 *= -1f;
		}
		if (num4 < num3)
		{
			num3 = num4;
		}
		num4 = A - num2;
		if (num4 < 0f)
		{
			num4 *= -1f;
		}
		if (num4 < num3)
		{
			num3 = num4;
		}
		return num3;
	}

	public static float MagnitudeHorizontal(this Vector3 v)
	{
		return (float)Math.Sqrt(v.x * v.x + v.z * v.z);
	}

	public static float MagnitudeHorizontalSquared(this Vector3 v)
	{
		return v.x * v.x + v.z * v.z;
	}

	public static bool LinesIntersect(Vector3 line1V1, Vector3 line1V2, Vector3 line2V1, Vector3 line2V2)
	{
		float num = line1V2.z - line1V1.z;
		float num2 = line1V1.x - line1V2.x;
		float num3 = num * line1V1.x + num2 * line1V1.z;
		float num4 = line2V2.z - line2V1.z;
		float num5 = line2V1.x - line2V2.x;
		float num6 = num4 * line2V1.x + num5 * line2V1.z;
		float num7 = num * num5 - num4 * num2;
		if (num7 == 0f)
		{
			return false;
		}
		float num8 = (num5 * num3 - num2 * num6) / num7;
		float num9 = (num * num6 - num4 * num3) / num7;
		if ((num8 > line1V1.x && num8 > line1V2.x) || (num8 > line2V1.x && num8 > line2V2.x) || (num8 < line1V1.x && num8 < line1V2.x) || (num8 < line2V1.x && num8 < line2V2.x) || (num9 > line1V1.z && num9 > line1V2.z) || (num9 > line2V1.z && num9 > line2V2.z) || (num9 < line1V1.z && num9 < line1V2.z) || (num9 < line2V1.z && num9 < line2V2.z))
		{
			return false;
		}
		return true;
	}

	public static bool IntersectLineCircle(Vector2 CircleCenter, float CircleRadius, Vector2 LineA, Vector2 LineB)
	{
		Vector2 lhs = CircleCenter - LineA;
		Vector2 vector = LineB - LineA;
		float num = Vector2.Dot(vector, vector);
		float num2 = Vector2.Dot(lhs, vector);
		float num3 = num2 / num;
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		else if (num3 > 1f)
		{
			num3 = 1f;
		}
		Vector2 vector2 = vector * num3 + LineA - CircleCenter;
		float num4 = Vector2.Dot(vector2, vector2);
		return num4 <= CircleRadius * CircleRadius;
	}
}
