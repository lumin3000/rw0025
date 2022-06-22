using System;
using System.Collections.Generic;
using UnityEngine;

public static class GenMap
{
	private static Material[] LowAirMaterials;

	static GenMap()
	{
		LowAirMaterials = new Material[101];
		for (int i = 0; i < 101; i++)
		{
			Color col = new Color(1f, 0f, 0f, 0.3f - (float)i / 100f * 0.3f);
			LowAirMaterials[i] = GenRender.SolidColorMaterial(col);
		}
	}

	public static void ReclaimResourcesFor(Thing OldThing)
	{
		EntityDefinition entityDefinition = null;
		float num;
		if (OldThing.def.eType == EntityType.Blueprint)
		{
			entityDefinition = ((Blueprint)OldThing).def.entityDefToBuild;
			num = 1f;
		}
		else if (OldThing.def.eType == EntityType.BuildingFrame)
		{
			entityDefinition = ((BuildingFrame)OldThing).def.entityDefToBuild;
			num = 1f;
		}
		else
		{
			entityDefinition = OldThing.def;
			num = 0.5f;
		}
		foreach (ResourceCost cost in entityDefinition.costList)
		{
			Find.ResourceManager.Gain(cost.rType, (int)Math.Ceiling((float)cost.Amount * num));
		}
	}

	public static Material GetAirMaterial(float OxyAmount)
	{
		return LowAirMaterials[(int)(OxyAmount * 100f)];
	}

	public static float AirPressure(this IntVec3 Sq)
	{
		if (DebugSettings.worldBreathable)
		{
			return 1f;
		}
		Room roomAt = Find.Grids.GetRoomAt(Sq);
		if (roomAt != null)
		{
			return roomAt.AirPressure;
		}
		if (Find.Grids.SquareContains(Sq, EntityType.Door))
		{
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 item in Sq.AdjacentSquares8Way())
			{
				if (!Find.Grids.HasBarrierAt(item))
				{
					list.Add(item);
				}
			}
			list = Gen.CombineGroupedSquares(list);
			float num = 0f;
			foreach (IntVec3 item2 in list)
			{
				num += item2.AirPressure();
			}
			return num / (float)list.Count;
		}
		return 0f;
	}

	public static bool HasAir(this Thing t)
	{
		if (DebugSettings.worldBreathable)
		{
			return true;
		}
		return t.Position.HasAir();
	}

	public static bool HasAir(this IntVec3 Pos)
	{
		if (DebugSettings.worldBreathable)
		{
			return true;
		}
		return Pos.AirPressure() >= 0.5f;
	}

	public static IntVec3 SpotToDropHaulableCloseTo(IntVec3 Loc)
	{
		IntVec3[] radialPattern = Gen.RadialPattern;
		foreach (IntVec3 intVec in radialPattern)
		{
			IntVec3 intVec2 = Loc + intVec;
			if (intVec2.Standable() && !Find.Grids.SquareContains(intVec2, EntityCategory.SmallObject) && GenGrid.LineOfSight(Loc, intVec2))
			{
				return intVec2;
			}
		}
		Debug.LogWarning("Could not find haulable drop spot close to " + Loc);
		return Loc;
	}

	public static IntVec3 RandomMapSquare()
	{
		return new IntVec3(UnityEngine.Random.Range(0, Find.Map.Size.x), 0, UnityEngine.Random.Range(0, Find.Map.Size.z));
	}

	public static IntVec3 RandomMapEdgeSquare()
	{
		IntVec3 result = default(IntVec3);
		if (UnityEngine.Random.value < 0.5f)
		{
			if (UnityEngine.Random.value < 0.5f)
			{
				result.x = 0;
			}
			else
			{
				result.x = Find.Map.Size.x - 1;
			}
			result.z = UnityEngine.Random.Range(0, Find.Map.Size.z);
		}
		else
		{
			if (UnityEngine.Random.value < 0.5f)
			{
				result.z = 0;
			}
			else
			{
				result.z = Find.Map.Size.z - 1;
			}
			result.x = UnityEngine.Random.Range(0, Find.Map.Size.x);
		}
		return result;
	}

	public static IntVec3 RandomEdgeSquareWith(Predicate<IntVec3> validator)
	{
		bool succeeded;
		return RandomEdgeSquareWith(validator, out succeeded);
	}

	public static IntVec3 RandomEdgeSquareWith(Predicate<IntVec3> validator, out bool succeeded)
	{
		int num = 0;
		IntVec3 intVec;
		do
		{
			intVec = RandomMapEdgeSquare();
			if (validator(intVec))
			{
				succeeded = true;
				return intVec;
			}
			num++;
		}
		while (num <= 100);
		succeeded = false;
		return intVec;
	}

	public static IntVec3 RandomSquareWith(Predicate<IntVec3> validator)
	{
		int num = 0;
		IntVec3 intVec;
		do
		{
			intVec = RandomMapSquare();
			if (validator(intVec))
			{
				return intVec;
			}
			num++;
		}
		while (num <= 100);
		return intVec;
	}

	public static IntVec3 RandomStandableLOSSquareNear(IntVec3 loc, int squareRadius)
	{
		Predicate<IntVec3> validator = (IntVec3 sq) => sq.InBounds() && sq.Standable() && !Find.PawnDestinationManager.DestinationIsReserved(sq) && GenGrid.LineOfSight(loc, sq);
		bool succeeded;
		return RandomMapSquareNear(loc, squareRadius, validator, out succeeded);
	}

	public static IntVec3 RandomMapSquareNear(IntVec3 searchRoot, int squareRadius, Predicate<IntVec3> validator, out bool succeeded)
	{
		int num = searchRoot.x - squareRadius;
		int num2 = searchRoot.x + squareRadius;
		int num3 = searchRoot.z - squareRadius;
		int num4 = searchRoot.z + squareRadius;
		if (num < 0)
		{
			num = 0;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num2 > Find.Map.Size.x)
		{
			num2 = Find.Map.Size.x;
		}
		if (num4 > Find.Map.Size.z)
		{
			num4 = Find.Map.Size.z;
		}
		int num5 = 0;
		do
		{
			IntVec3 intVec = new IntVec3(UnityEngine.Random.Range(num, num2 + 1), 0, UnityEngine.Random.Range(num3, num4 + 1));
			if (validator == null || validator(intVec))
			{
				succeeded = true;
				return intVec;
			}
			num5++;
		}
		while (num5 <= 20);
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = num; i <= num2; i++)
		{
			list.Add(i);
		}
		for (int j = num3; j <= num4; j++)
		{
			list2.Add(j);
		}
		list.Shuffle();
		list2.Shuffle();
		for (int k = 0; k < list.Count; k++)
		{
			for (int l = 0; l < list2.Count; l++)
			{
				IntVec3 intVec = new IntVec3(list[k], 0, list2[l]);
				if (validator(intVec))
				{
					succeeded = true;
					return intVec;
				}
			}
		}
		succeeded = false;
		return searchRoot;
	}

	public static IntVec3 SpotNearForResourceSpawn(IntVec3 Pos)
	{
		int num = Gen.NumSquaresInRadius(10f);
		for (int i = 1; i < num; i++)
		{
			IntVec3 intVec = Pos + Gen.RadialPattern[i];
			if (intVec.Standable())
			{
				return intVec;
			}
		}
		Debug.LogWarning(string.Concat("Failed to find resource spawn spot near ", Pos, "."));
		return Pos;
	}
}
