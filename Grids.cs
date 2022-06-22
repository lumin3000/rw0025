using System.Collections.Generic;
using UnityEngine;

public class Grids
{
	public Thing[,,] blockerGrid;

	public Thing[,,] barrierGrid;

	private Room[,,] roomGrid;

	private List<Thing>[,,] thingGrid;

	public Grids()
	{
		blockerGrid = new Thing[Find.Map.Size.x, 1, Find.Map.Size.z];
		barrierGrid = new Thing[Find.Map.Size.x, 1, Find.Map.Size.z];
		roomGrid = new Room[Find.Map.Size.x, 1, Find.Map.Size.z];
		thingGrid = new List<Thing>[Find.Map.Size.x, 1, Find.Map.Size.z];
		for (int i = 0; i < Find.Map.Size.x; i++)
		{
			for (int j = 0; j < Find.Map.Size.y; j++)
			{
				for (int k = 0; k < Find.Map.Size.z; k++)
				{
					thingGrid[i, j, k] = new List<Thing>();
				}
			}
		}
	}

	public void RegisterInGrids(Thing t)
	{
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(t))
		{
			if (!item.InBounds())
			{
				Debug.LogWarning(string.Concat(t, " tried to register out of bounds at ", item, ". Destroying."));
				t.Destroy();
				break;
			}
			thingGrid[item.x, item.y, item.z].Add(t);
			if (t.def.IsBlocker)
			{
				RegisterInBlockerMapAt(t, item);
			}
			if (t.def.isBarrier)
			{
				RegisterInBarrierMapAt(t, item);
			}
		}
	}

	public void DeRegisterInGrids(Thing t)
	{
		if (!t.spawnedInWorld)
		{
			return;
		}
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(t))
		{
			if (!item.InBounds())
			{
				Debug.LogWarning(string.Concat(t, " tried to de-register out of bounds at ", item, ". Destroying."));
				t.Destroy();
				break;
			}
			if (thingGrid[item.x, item.y, item.z].Contains(t))
			{
				thingGrid[item.x, item.y, item.z].Remove(t);
			}
			ClearFromBlockerMapAt(t, item);
			if (t.def.isBarrier)
			{
				ClearFromBarrierMap(t);
			}
		}
	}

	public List<Thing> ThingsAt(IntVec3 sq)
	{
		//Discarded unreachable code: IL_0031, IL_007e
		if (sq.InBounds())
		{
			try
			{
				return thingGrid[sq.x, sq.y, sq.z];
			}
			catch
			{
				Debug.LogError(string.Concat("Bad coords ", sq, ". thingGrid size is ", thingGrid.Length));
				return new List<Thing>();
			}
		}
		return new List<Thing>();
	}

	public bool SquareContains(IntVec3 Square, EntityType TType)
	{
		return ThingAt(Square, TType) != null;
	}

	public Thing ThingAt(IntVec3 Square, EntityType TType)
	{
		foreach (Thing item in ThingsAt(Square))
		{
			if (item.def.eType == TType)
			{
				return item;
			}
		}
		return null;
	}

	public bool SquareContains(IntVec3 Square, EntityCategory TCat)
	{
		return ThingAt(Square, TCat) != null;
	}

	public Thing ThingAt(IntVec3 Square, EntityCategory TCat)
	{
		foreach (Thing item in ThingsAt(Square))
		{
			if (item.def.category == TCat)
			{
				return item;
			}
		}
		return null;
	}

	public T ThingAt<T>(IntVec3 Square) where T : Thing
	{
		foreach (Thing item in ThingsAt(Square))
		{
			T val = item as T;
			if (val != null)
			{
				return val;
			}
		}
		return (T)null;
	}

	public Thing BlockerAt(IntVec3 loc)
	{
		return blockerGrid[loc.x, loc.y, loc.z];
	}

	protected void RegisterInBlockerMapAt(Thing newBlock, IntVec3 pos)
	{
		if (BlockerAt(pos) != null && !BlockerAt(pos).destroyed)
		{
			Debug.LogWarning(string.Concat("Added blocker ", newBlock.Label, " over blocker ", BlockerAt(pos).Label, " at ", pos, ". Destroying old blocker."));
			BlockerAt(pos).Destroy();
		}
		else
		{
			blockerGrid[pos.x, pos.y, pos.z] = newBlock;
			Find.Map.mapDrawer.MapChanged(pos, MapChangeType.Blockers);
			Find.GlowGrid.MarkGlowGridDirty(pos);
		}
	}

	protected void ClearFromBlockerMapAt(Thing oldBlock, IntVec3 pos)
	{
		if (BlockerAt(pos) == oldBlock)
		{
			blockerGrid[pos.x, pos.y, pos.z] = null;
			Find.Map.mapDrawer.MapChanged(pos, MapChangeType.Blockers);
			Find.GlowGrid.MarkGlowGridDirty(pos);
		}
	}

	public bool HasBarrierAt(IntVec3 loc)
	{
		if (!loc.InBounds())
		{
			return true;
		}
		return barrierGrid[loc.x, loc.y, loc.z] != null;
	}

	protected void RegisterInBarrierMapAt(Thing newBlock, IntVec3 Pos)
	{
		if (HasBarrierAt(Pos) && !GetBarrierAt(Pos).destroyed)
		{
			Debug.LogWarning(string.Concat("Tried to add one Barrier (", newBlock.Label, ") over another (", GetBarrierAt(Pos).Label, ") at ", Pos, "."));
		}
		else
		{
			barrierGrid[Pos.x, Pos.y, Pos.z] = newBlock;
		}
	}

	protected void ClearFromBarrierMap(Thing oldBar)
	{
		if (GetBarrierAt(oldBar.Position) == oldBar)
		{
			barrierGrid[oldBar.Position.x, oldBar.Position.y, oldBar.Position.z] = null;
		}
	}

	private Thing GetBarrierAt(IntVec3 Loc)
	{
		return barrierGrid[Loc.x, Loc.y, Loc.z];
	}

	public void RegisterInRoomMap(Room e)
	{
		foreach (IntVec3 squares in e.squaresList)
		{
			if (GetRoomAt(squares) != null)
			{
				Debug.LogWarning(string.Concat("Tried to add one Room  over another at ", squares, "."));
				break;
			}
			roomGrid[squares.x, squares.y, squares.z] = e;
		}
	}

	public void DeRegisterInRoomMap(Room e)
	{
		foreach (IntVec3 squares in e.squaresList)
		{
			if (GetRoomAt(squares) != e)
			{
				Debug.LogError("Clearing wrong room at " + squares);
			}
			roomGrid[squares.x, squares.y, squares.z] = null;
		}
	}

	public Room GetRoomAt(IntVec3 loc)
	{
		if (!loc.InBounds())
		{
			return null;
		}
		return roomGrid[loc.x, loc.y, loc.z];
	}
}
