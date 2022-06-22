using UnityEngine;

public static class GenMapGen
{
	public static IntVec3 RandomWalkablePlacementSpot()
	{
		int newX;
		int newZ;
		do
		{
			newX = Random.Range(0, Find.Map.Size.x);
			newZ = Random.Range(0, Find.Map.Size.z);
		}
		while (!new IntVec3(newX, 0, newZ).Walkable());
		return new IntVec3(newX, 0, newZ);
	}

	public static IntVec3 RandomWalkablePlacementSpot_NotEdge(int MinEdgeDistance)
	{
		int num = 0;
		IntVec3 intVec;
		do
		{
			intVec = RandomSpot_NotEdge(MinEdgeDistance);
			if (intVec.Standable())
			{
				return intVec;
			}
			num++;
		}
		while (num <= 200);
		Debug.LogWarning("Failed to find walkable placement spot not edge.");
		return intVec;
	}

	public static IntVec3 RandomSpot_NotEdge(int MinEdgeDistance)
	{
		int newX = Random.Range(MinEdgeDistance, Find.Map.Size.x - MinEdgeDistance);
		int newZ = Random.Range(MinEdgeDistance, Find.Map.Size.z - MinEdgeDistance);
		return new IntVec3(newX, 0, newZ);
	}
}
