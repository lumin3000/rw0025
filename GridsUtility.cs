using System.Collections.Generic;

public static class GridsUtility
{
	public static List<Thing> ThingsInSquare(this IntVec3 Loc)
	{
		return Find.Grids.ThingsAt(Loc);
	}

	public static bool IsInPrisonCell(this IntVec3 Sq)
	{
		return Find.Grids.GetRoomAt(Sq)?.IsPrisonCell ?? false;
	}
}
