public static class ReachabilityUtility
{
	public static bool CanReach(this Thing searcher, IntVec3 loc)
	{
		return searcher.CanReach(loc, adjacentIsOK: false);
	}

	public static bool CanReach(this Thing searcher, TargetPack dest, bool adjacentIsOK)
	{
		return Find.ReachabilityRegions.ReachableBetween(searcher.Position, dest, adjacentIsOK);
	}

	public static bool CanReachForInteract(this Thing searcher, Thing target)
	{
		if (target.Position.Walkable())
		{
			return searcher.CanReach(target.Position);
		}
		return searcher.CanReach(target, adjacentIsOK: true);
	}

	public static int ContainingRegion(this IntVec3 loc)
	{
		return Find.ReachabilityRegions.RegionIndexAt(loc);
	}
}
