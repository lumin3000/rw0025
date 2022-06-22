using System;
using UnityEngine;

public static class DropPodUtility
{
	private const int DropPodRandomRadius = 4;

	public static void MakeDropPodAt(IntVec3 Pos, DropPodContentsInfo Contents)
	{
		DropPodIncoming dropPodIncoming = (DropPodIncoming)ThingMaker.MakeThing(EntityType.DropPodIncoming);
		dropPodIncoming.contents = Contents;
		ThingMaker.Spawn(dropPodIncoming, Pos);
	}

	public static IntVec3 DropPodSpotNear(IntVec3 loc)
	{
		return DropPodSpotNear(loc, 4);
	}

	public static IntVec3 DropPodSpotNear(IntVec3 loc, int radius)
	{
		Predicate<IntVec3> validator = (IntVec3 sq) => sq.InBounds() && sq.Standable() && !Find.RoofGrid.Roofed(sq) && !Find.Grids.SquareContains(sq, EntityCategory.SmallObject) && !Find.Grids.SquareContains(sq, EntityType.DropPodIncoming) && !Find.Grids.SquareContains(sq, EntityType.DropPod) && Find.ReachabilityRegions.ReachableBetween(loc, sq, adjacentIsOK: false);
		bool succeeded;
		IntVec3 result = GenMap.RandomMapSquareNear(loc, radius, validator, out succeeded);
		if (!succeeded)
		{
			Debug.LogWarning(string.Concat("Did not find pod drop spot near ", loc, "."));
			return loc;
		}
		return result;
	}
}
