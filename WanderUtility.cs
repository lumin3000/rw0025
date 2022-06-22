public static class WanderUtility
{
	public static bool InSameRoom(IntVec3 locA, IntVec3 locB)
	{
		if (Find.Grids.GetRoomAt(locB) != Find.Grids.GetRoomAt(locA))
		{
			return false;
		}
		if (Find.Grids.HasBarrierAt(locB))
		{
			return false;
		}
		return true;
	}
}
