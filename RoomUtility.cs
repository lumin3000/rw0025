public static class RoomUtility
{
	public static Room ContainingRoom(this IntVec3 pos)
	{
		return Find.Grids.GetRoomAt(pos);
	}

	public static Room ContainingRoom(this Thing t)
	{
		return Find.Grids.GetRoomAt(t.Position);
	}
}
