using System;
using System.Linq;
using UnityEngine;

public static class GenGrid
{
	public static bool InBounds(this IntVec3 Sq)
	{
		if (Sq.x < 0 || Sq.z < 0 || Sq.x > Find.Map.Size.x - 1 || Sq.z > Find.Map.Size.z - 1)
		{
			return false;
		}
		return true;
	}

	public static bool InBounds(this Vector3 v)
	{
		if (v.x < 0f || v.z < 0f || v.x >= (float)Find.Map.Size.x || v.z >= (float)Find.Map.Size.z)
		{
			return false;
		}
		return true;
	}

	public static bool LineOfSight(IntVec3 start, IntVec3 end)
	{
		return LineOfSight(start, end, skipFirstSquare: false);
	}

	public static bool LineOfSight(IntVec3 Start, IntVec3 End, bool skipFirstSquare)
	{
		if (!Start.InBounds() || !End.InBounds())
		{
			return false;
		}
		int num = Math.Abs(End.x - Start.x);
		int num2 = Math.Abs(End.z - Start.z);
		int num3 = Start.x;
		int num4 = Start.z;
		int num5 = 1 + num + num2;
		int num6 = ((End.x > Start.x) ? 1 : (-1));
		int num7 = ((End.z > Start.z) ? 1 : (-1));
		int num8 = num - num2;
		num *= 2;
		num2 *= 2;
		IntVec3 intVec = default(IntVec3);
		while (num5 > 1)
		{
			intVec.x = num3;
			intVec.z = num4;
			if ((!skipFirstSquare || !(intVec == Start)) && !intVec.CanBeSeenOverFast())
			{
				return false;
			}
			if (num8 > 0)
			{
				num3 += num6;
				num8 -= num2;
			}
			else
			{
				num4 += num7;
				num8 += num;
			}
			num5--;
		}
		return true;
	}

	public static bool Walkable(this IntVec3 Loc)
	{
		return Loc.Walkable(PathParameters.smart);
	}

	public static bool Walkable(this IntVec3 Loc, PathingParameters Params)
	{
		if (!Loc.InBounds())
		{
			return false;
		}
		Thing thing = Find.Grids.BlockerAt(Loc);
		if (thing != null && thing.def.passability == Traversability.Impassable)
		{
			return false;
		}
		return true;
	}

	public static bool Standable(this IntVec3 loc)
	{
		if (!loc.Walkable())
		{
			return false;
		}
		foreach (Thing item in Find.Grids.ThingsAt(loc))
		{
			if (item.def.passability != 0)
			{
				return false;
			}
		}
		return true;
	}

	public static bool Isolated(this IntVec3 loc)
	{
		if (!Find.Map.initialized)
		{
			return !Find.ReachabilityRegions.ReachableBetween(loc, Genner_PlayerStuff.PlayerStartSpot, adjacentIsOK: false);
		}
		foreach (Thing item in Find.PawnManager.ColonistsAndPrisoners.Cast<Thing>().Concat(Find.BuildingManager.AllBuildingsColonist.Cast<Thing>()))
		{
			if (Find.ReachabilityRegions.ReachableBetween(loc, item, adjacentIsOK: true))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanBeSeenOver(this IntVec3 Loc)
	{
		if (!Loc.InBounds())
		{
			return false;
		}
		if (Find.Grids.BlockerAt(Loc) != null && !Find.Grids.BlockerAt(Loc).def.canBeSeenOver)
		{
			return false;
		}
		return true;
	}

	public static bool CanBeSeenOverFast(this IntVec3 Loc)
	{
		if (Find.Grids.BlockerAt(Loc) != null && !Find.Grids.BlockerAt(Loc).def.canBeSeenOver)
		{
			return false;
		}
		return true;
	}
}
