using UnityEngine;

public static class IntVec3Utility
{
	public static IntVec3 ToIntVec3(this Vector3 vect)
	{
		return new IntVec3(vect);
	}
}
