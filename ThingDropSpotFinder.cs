public static class ThingDropSpotFinder
{
	public enum DropSpotQuality
	{
		Bad,
		Okay,
		Perfect
	}

	public static IntVec3 BestDropSpotNear(IntVec3 root)
	{
		DropSpotQuality dropSpotQuality = DropSpotQuality.Bad;
		IntVec3 result = root;
		for (int i = 0; i < 9; i++)
		{
			IntVec3 intVec = root + Gen.ManualRadialPattern[i];
			if (intVec.InBounds() && intVec.Standable())
			{
				DropSpotQuality dropSpotQuality2 = DropSpotQualityAt(intVec);
				if (dropSpotQuality2 > dropSpotQuality)
				{
					result = intVec;
					dropSpotQuality = dropSpotQuality2;
				}
			}
		}
		return result;
	}

	public static DropSpotQuality DropSpotQualityAt(IntVec3 loc)
	{
		DropSpotQuality result = DropSpotQuality.Perfect;
		foreach (Thing item in Find.Grids.ThingsAt(loc))
		{
			if (item.def.eType == EntityType.Door)
			{
				return DropSpotQuality.Bad;
			}
			if (item.def.eType == EntityType.Pawn)
			{
				if (((Pawn)item).Incapacitated)
				{
					return DropSpotQuality.Bad;
				}
				result = DropSpotQuality.Okay;
			}
			else if (item.def.selectable)
			{
				return DropSpotQuality.Bad;
			}
		}
		return result;
	}
}
