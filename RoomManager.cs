using System.Collections.Generic;
using FloodFill;

public class RoomManager
{
	public List<Room> allRooms = new List<Room>();

	protected List<IntVec3> barrierSpawnedLocs = new List<IntVec3>();

	protected List<IntVec3> barrierRemovedLocs = new List<IntVec3>();

	protected bool RoomCheckingSuspended => !Find.Map.initialized;

	public void DrawRooms()
	{
		foreach (Room allRoom in allRooms)
		{
			allRoom.RoomDraw();
		}
	}

	public void RoomManagerOnGUI()
	{
		foreach (Room allRoom in allRooms)
		{
			allRoom.RoomOnGUI();
		}
	}

	public void Expose()
	{
		Scribe.EnterNode("RoomManager");
		Scribe.LookList(ref allRooms, "RoomList");
		Scribe.ExitNode();
	}

	public void ResolveRoomChangesUpdate_First()
	{
		foreach (IntVec3 barrierRemovedLoc in barrierRemovedLocs)
		{
			foreach (IntVec3 item in barrierRemovedLoc.AdjacentSquaresCardinal())
			{
				if (Find.Grids.GetRoomAt(item) != null)
				{
					Find.Grids.GetRoomAt(item).Delete();
				}
			}
		}
		foreach (IntVec3 barrierSpawnedLoc in barrierSpawnedLocs)
		{
			Find.Grids.GetRoomAt(barrierSpawnedLoc)?.Delete();
		}
		List<IntVec3> list = new List<IntVec3>();
		foreach (IntVec3 barrierSpawnedLoc2 in barrierSpawnedLocs)
		{
			foreach (IntVec3 item2 in barrierSpawnedLoc2.AdjacentSquares8Way())
			{
				if (Find.Grids.GetRoomAt(item2) == null && !Find.Grids.HasBarrierAt(item2) && !list.Contains(item2))
				{
					list.Add(item2);
				}
			}
		}
		TryMakeRoomsFromList(list);
		foreach (IntVec3 barrierRemovedLoc2 in barrierRemovedLocs)
		{
			if (Find.Grids.GetRoomAt(barrierRemovedLoc2) == null && !Find.Grids.HasBarrierAt(barrierRemovedLoc2))
			{
				TryMakeRoomFrom(barrierRemovedLoc2);
			}
		}
		barrierSpawnedLocs.Clear();
		barrierRemovedLocs.Clear();
	}

	public void BarrierSpawned(Thing newBarrier)
	{
		if (!RoomCheckingSuspended)
		{
			barrierSpawnedLocs.Add(newBarrier.Position);
		}
	}

	public void BarrierRemovedAt(IntVec3 sq)
	{
		if (!RoomCheckingSuspended)
		{
			barrierRemovedLocs.Add(sq);
		}
	}

	protected void TryMakeRoomsFromList(List<IntVec3> LocList)
	{
		List<IntVec3> list = Gen.CombineGroupedSquares(LocList);
		foreach (IntVec3 item in list)
		{
			TryMakeRoomFrom(item);
		}
	}

	protected void TryMakeRoomFrom(IntVec3 loc)
	{
		if (Find.Grids.GetRoomAt(loc) == null)
		{
			List<IntVec3> list = MapFloodFiller.SquaresEnclosedWith(loc);
			if (list != null)
			{
				allRooms.Add(new Room(list));
			}
		}
	}
}
