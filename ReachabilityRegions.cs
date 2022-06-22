using System.Collections.Generic;
using FloodFill;
using UnityEngine;

public class ReachabilityRegions
{
	public const int NotSet = -1;

	private const int MaxNumZones = 50000;

	private int[,,] reachGrid;

	private bool gridIsDirty;

	private bool regionsEverMade;

	public ReachabilityRegions()
	{
		Reset();
	}

	private void Reset()
	{
		reachGrid = new int[Find.Map.Size.x, 1, Find.Map.Size.z];
		for (int i = 0; i < Find.Map.Size.x; i++)
		{
			for (int j = 0; j < Find.Map.Size.y; j++)
			{
				for (int k = 0; k < Find.Map.Size.z; k++)
				{
					reachGrid[i, j, k] = -1;
				}
			}
		}
	}

	public void RemakeAllRegions()
	{
		if (reachGrid == null)
		{
			Reset();
		}
		int maxUsedIndex = MapFloodFiller.maxUsedIndex;
		Thing[,,] blockerGrid = Find.Grids.blockerGrid;
		for (int i = 0; i < Find.Map.Size.x; i++)
		{
			for (int j = 0; j < Find.Map.Size.y; j++)
			{
				for (int k = 0; k < Find.Map.Size.z; k++)
				{
					if (reachGrid[i, j, k] <= maxUsedIndex)
					{
						Thing thing = blockerGrid[i, j, k];
						if (thing == null || thing.def.passability != Traversability.Impassable)
						{
							MakeRegionFrom(new IntVec3(i, j, k));
						}
					}
				}
			}
		}
		regionsEverMade = true;
	}

	private void MakeRegionFrom(IntVec3 root)
	{
		MapFloodFiller.MakeReachabilityRegionFrom(root, reachGrid);
	}

	public void Notify_ImpassableThingRemoved(Thing thing)
	{
		gridIsDirty = true;
	}

	public void Notify_ImpassableThingAdded(Thing thing)
	{
		gridIsDirty = true;
	}

	public bool ReachableBetween(IntVec3 start, TargetPack targ, bool adjacentIsOK)
	{
		if (DebugSettings.drawReachabilityChecks)
		{
			Find.DebugDrawer.MakeDebugSquare(start, "root", 5, 100);
		}
		if (!regionsEverMade)
		{
			RemakeAllRegions();
		}
		if (!start.InBounds())
		{
			return false;
		}
		IEnumerable<IntVec3> enumerable;
		if (targ.thing != null)
		{
			enumerable = Gen.AdjacentSquares8Way(targ.thing);
		}
		else
		{
			List<IntVec3> list = new List<IntVec3>();
			list.Add(targ.Loc);
			if (adjacentIsOK)
			{
				foreach (IntVec3 item in targ.Loc.AdjacentSquares8Way())
				{
					list.Add(item);
				}
			}
			enumerable = list;
		}
		int num = reachGrid[start.x, start.y, start.z];
		foreach (IntVec3 item2 in enumerable)
		{
			if (!item2.InBounds())
			{
				if (DebugSettings.drawReachabilityChecks)
				{
					Find.DebugDrawer.MakeDebugSquare(item2, "bnds", 25, 100);
				}
				continue;
			}
			if (!item2.Walkable())
			{
				if (DebugSettings.drawReachabilityChecks)
				{
					Find.DebugDrawer.MakeDebugSquare(item2, "walk", 55, 100);
				}
				continue;
			}
			int num2 = reachGrid[item2.x, item2.y, item2.z];
			if (num != num2)
			{
				if (DebugSettings.drawReachabilityChecks)
				{
					Find.DebugDrawer.MakeDebugSquare(item2, "reg", 75, 100);
				}
				continue;
			}
			if (DebugSettings.drawReachabilityChecks)
			{
				Find.DebugDrawer.MakeDebugSquare(item2, "ok", 90, 100);
			}
			return true;
		}
		return false;
	}

	public int RegionIndexAt(IntVec3 loc)
	{
		if (!loc.InBounds())
		{
			return -999;
		}
		return reachGrid[loc.x, loc.y, loc.z];
	}

	public void ReachabilityRegionsTick()
	{
		if (gridIsDirty)
		{
			RemakeAllRegions();
			gridIsDirty = false;
		}
	}

	public void DebugDrawReachability()
	{
		if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			RemakeAllRegions();
		}
		if (!DebugSettings.drawReachability)
		{
			return;
		}
		for (int i = 0; i < Find.Map.Size.x; i++)
		{
			for (int j = 0; j < Find.Map.Size.y; j++)
			{
				for (int k = 0; k < Find.Map.Size.z; k++)
				{
					int num = reachGrid[i, j, k];
					if (num != -1)
					{
						IntVec3 sq = new IntVec3(i, j, k);
						DebugRender.RenderSquare(sq, num);
					}
				}
			}
		}
	}
}
