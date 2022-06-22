using System;
using UnityEngine;

public struct IntVec2
{
	public int x;

	public int z;

	public Vector3 ToVector3 => new Vector3(x, 0f, z);

	public IntVec2(int newX, int newZ)
	{
		x = newX;
		z = newZ;
	}

	public IntVec2(Vector2 v2)
	{
		x = (int)v2.x;
		z = (int)v2.y;
	}

	public Vector2 ToVector2()
	{
		return new Vector2(x, z);
	}

	public override string ToString()
	{
		return "(" + x + ", " + z + ")";
	}

	public static IntVec2 FromString(string Str)
	{
		Str = Str.TrimStart('(');
		Str = Str.TrimEnd(')');
		string[] array = Str.Split(',');
		int newX = Convert.ToInt32(array[0]);
		int newZ = Convert.ToInt32(array[1]);
		return new IntVec2(newX, newZ);
	}

	public override bool Equals(object o)
	{
		//Discarded unreachable code: IL_0017, IL_0024
		try
		{
			return this == (IntVec2)o;
		}
		catch
		{
			return false;
		}
	}

	public override int GetHashCode()
	{
		return x + z * 500;
	}

	public static IntVec2 operator +(IntVec2 a, IntVec2 b)
	{
		return new IntVec2(a.x + b.x, a.z + b.z);
	}

	public static IntVec2 operator -(IntVec2 a, IntVec2 b)
	{
		return new IntVec2(a.x - b.x, a.z - b.z);
	}

	public static IntVec2 operator *(IntVec2 a, int b)
	{
		return new IntVec2(a.x * b, a.z * b);
	}

	public static IntVec2 operator /(IntVec2 a, int b)
	{
		return new IntVec2(a.x / b, a.z / b);
	}

	public static bool operator ==(IntVec2 a, IntVec2 b)
	{
		if (a.x == b.x && a.z == b.z)
		{
			return true;
		}
		return false;
	}

	public static bool operator !=(IntVec2 a, IntVec2 b)
	{
		if (a.x != b.x || a.z != b.z)
		{
			return true;
		}
		return false;
	}
}
