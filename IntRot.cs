using System;
using UnityEngine;

public struct IntRot
{
	private int RotInt;

	public int AsInt
	{
		get
		{
			return RotInt;
		}
		set
		{
			if (value < 0)
			{
				value += 4000;
			}
			RotInt = value % 4;
		}
	}

	public float AsAngle => AsInt switch
	{
		0 => 0f, 
		1 => 90f, 
		2 => 180f, 
		3 => 270f, 
		_ => 0f, 
	};

	public Quaternion AsQuat
	{
		get
		{
			switch (AsInt)
			{
			case 0:
				return Quaternion.identity;
			case 1:
				return Quaternion.LookRotation(Vector3.right);
			case 2:
				return Quaternion.LookRotation(Vector3.back);
			case 3:
				return Quaternion.LookRotation(Vector3.left);
			default:
				Debug.LogError("ToQuat with Rot = " + AsInt);
				return Quaternion.identity;
			}
		}
	}

	public bool IsHorizontal => RotInt == 1 || RotInt == 3;

	public IntVec3 FacingSquare => AsInt switch
	{
		0 => new IntVec3(0, 0, 1), 
		1 => new IntVec3(1, 0, 0), 
		2 => new IntVec3(0, 0, -1), 
		3 => new IntVec3(-1, 0, 0), 
		_ => default(IntVec3), 
	};

	public static IntRot north => new IntRot(0);

	public static IntRot east => new IntRot(1);

	public static IntRot south => new IntRot(2);

	public static IntRot west => new IntRot(3);

	public static IntRot random => new IntRot((int)UnityEngine.Random.Range(0f, 3.9999f));

	public IntRot(int newRot)
	{
		RotInt = newRot;
	}

	public void Rotate(RotationDirection RotDir)
	{
		if (RotDir == RotationDirection.Clockwise)
		{
			AsInt++;
		}
		if (RotDir == RotationDirection.Counterclockwise)
		{
			AsInt--;
		}
	}

	public static IntRot FromAngleFlat(float angle)
	{
		if (angle < 45f)
		{
			return north;
		}
		if (angle < 135f)
		{
			return east;
		}
		if (angle < 225f)
		{
			return south;
		}
		if (angle < 315f)
		{
			return west;
		}
		return north;
	}

	public static IntRot FromIntVec3(IntVec3 offset)
	{
		if (offset.x == 1)
		{
			return east;
		}
		if (offset.x == -1)
		{
			return west;
		}
		if (offset.z == 1)
		{
			return north;
		}
		if (offset.z == -1)
		{
			return south;
		}
		Debug.LogError("FromIntVec3 with bad offset " + offset);
		return north;
	}

	public override bool Equals(object o)
	{
		//Discarded unreachable code: IL_001c, IL_0029
		try
		{
			IntRot intRot = (IntRot)o;
			return AsInt == intRot.AsInt;
		}
		catch
		{
			return false;
		}
	}

	public override int GetHashCode()
	{
		return AsInt;
	}

	public override string ToString()
	{
		return AsInt.ToString();
	}

	public static IntRot FromString(string Str)
	{
		return new IntRot(Convert.ToInt32(Str));
	}

	public static bool operator ==(IntRot a, IntRot b)
	{
		return a.AsInt == b.AsInt;
	}

	public static bool operator !=(IntRot a, IntRot b)
	{
		return a.AsInt != b.AsInt;
	}
}
