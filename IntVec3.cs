using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public struct IntVec3
{
	[XmlAttribute]
	public int x;

	[XmlIgnore]
	public int y;

	[XmlAttribute]
	public int z;

	public bool IsInvalid => y < -500;

	public bool IsValid => y >= -500;

	public static IntVec3 zero => new IntVec3(0, 0, 0);

	public static IntVec3 north => new IntVec3(0, 0, 1);

	public static IntVec3 east => new IntVec3(1, 0, 0);

	public static IntVec3 south => new IntVec3(0, 0, -1);

	public static IntVec3 west => new IntVec3(-1, 0, 0);

	public static IntVec3 Invalid => new IntVec3(-1000, -1000, -1000);

	public float LengthHorizontalSquared => x * x + z * z;

	public float LengthHorizontal => (float)Math.Sqrt(x * x + z * z);

	public float LengthHorizontalFast
	{
		get
		{
			float num = x * x + z * z;
			if (num <= 0.7f)
			{
				return 0f;
			}
			if (num <= 1.3f)
			{
				return 1f;
			}
			if (num <= 2.1f)
			{
				return 1.414f;
			}
			if (num <= 3.1f)
			{
				return 1.73f;
			}
			if (num <= 4.1f)
			{
				return 2f;
			}
			return Gen.SquareRootsByTenths[(int)Math.Round(num * 0.1f)] * 10f;
		}
	}

	public float AngleFlat
	{
		get
		{
			if (x == 0 && z == 0)
			{
				return 0f;
			}
			return Quaternion.LookRotation(ToVector3()).eulerAngles.y;
		}
	}

	public IntVec3(int newX, int newY, int newZ)
	{
		x = newX;
		y = newY;
		z = newZ;
	}

	public IntVec3(Vector3 v)
	{
		x = Mathf.FloorToInt(v.x);
		y = 0;
		z = Mathf.FloorToInt(v.z);
	}

	public IntVec3(Vector2 v)
	{
		x = Mathf.FloorToInt(v.x);
		y = 0;
		z = Mathf.FloorToInt(v.y);
	}

	public static IntVec3 FromString(string Str)
	{
		Str = Str.TrimStart('(');
		Str = Str.TrimEnd(')');
		string[] array = Str.Split(',');
		int newX = Convert.ToInt32(array[0]);
		int newY = Convert.ToInt32(array[1]);
		int newZ = Convert.ToInt32(array[2]);
		return new IntVec3(newX, newY, newZ);
	}

	public Vector3 ToVector3()
	{
		return new Vector3(x, y, z);
	}

	public Vector3 ToVector3Shifted()
	{
		return new Vector3((float)x + 0.5f, y, (float)z + 0.5f);
	}

	public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
	{
		return ToVector3ShiftedWithAltitude(Altitudes.AltitudeFor(AltLayer));
	}

	public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
	{
		return new Vector3((float)x + 0.5f, (float)y + AddedAltitude, (float)z + 0.5f);
	}

	public bool WithinHorizontalDistanceOf(IntVec3 OtherLoc, float Dist)
	{
		float num = x - OtherLoc.x;
		float num2 = z - OtherLoc.z;
		return num * num + num2 * num2 <= Dist * Dist;
	}

	public static IntVec3 FromVector3(Vector3 v)
	{
		return FromVector3(v, 0);
	}

	public static IntVec3 FromVector3(Vector3 v, int newY)
	{
		return new IntVec3((int)v.x, newY, (int)v.z);
	}

	public Vector2 ToScreenPosition()
	{
		Vector3 vector = Find.CameraMap.camera.WorldToScreenPoint(ToVector3Shifted());
		return new Vector2(vector.x, (float)Screen.height - vector.y);
	}

	public bool AdjacentToCardinal(IntVec3 other)
	{
		if (other.z == z && (other.x == x + 1 || other.x == x - 1))
		{
			return true;
		}
		if (other.x == x && (other.z == z + 1 || other.z == z - 1))
		{
			return true;
		}
		return false;
	}

	public bool AdjacentTo8Way(IntVec3 other)
	{
		int num = x - other.x;
		int num2 = z - other.z;
		if (num == 0 && num2 == 0)
		{
			return false;
		}
		if (num < 0)
		{
			num *= -1;
		}
		if (num2 < 0)
		{
			num2 *= -1;
		}
		return num <= 1 && num2 <= 1;
	}

	public bool AdjacentTo8WayOrInside(IntVec3 other)
	{
		int num = x - other.x;
		int num2 = z - other.z;
		if (num < 0)
		{
			num *= -1;
		}
		if (num2 < 0)
		{
			num2 *= -1;
		}
		return num <= 1 && num2 <= 1;
	}

	public IEnumerable<IntVec3> AdjacentSquaresCardinal()
	{
		IntVec3[] cardinalDirections = Gen.CardinalDirections;
		foreach (IntVec3 adj in cardinalDirections)
		{
			yield return this + adj;
		}
	}

	public IEnumerable<IntVec3> AdjacentSquares8Way()
	{
		IntVec3[] adjacentSquares = Gen.AdjacentSquares;
		foreach (IntVec3 Adj in adjacentSquares)
		{
			yield return this + Adj;
		}
	}

	public IEnumerable<IntVec3> AdjacentSquares8WayAndInside()
	{
		yield return this;
		IntVec3[] adjacentSquares = Gen.AdjacentSquares;
		foreach (IntVec3 Adj in adjacentSquares)
		{
			yield return this + Adj;
		}
	}

	public override bool Equals(object obj)
	{
		return obj is IntVec3 && this == (IntVec3)obj;
	}

	public override int GetHashCode()
	{
		int num = x;
		num ^= 397 * y;
		return num ^ (397 * z);
	}

	public override string ToString()
	{
		return "(" + x + ", " + y + ", " + z + ")";
	}

	public static IntVec3 operator +(IntVec3 a, IntVec3 b)
	{
		return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static IntVec3 operator -(IntVec3 a, IntVec3 b)
	{
		return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static IntVec3 operator *(IntVec3 a, int i)
	{
		return new IntVec3(a.x * i, a.y * i, a.z * i);
	}

	public static bool operator ==(IntVec3 a, IntVec3 b)
	{
		if (a.x == b.x && a.z == b.z && a.y == b.y)
		{
			return true;
		}
		return false;
	}

	public static bool operator !=(IntVec3 a, IntVec3 b)
	{
		if (a.x != b.x || a.z != b.z || a.y != b.y)
		{
			return true;
		}
		return false;
	}
}
