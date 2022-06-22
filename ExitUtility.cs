public static class ExitUtility
{
	public static IntVec3 ClosestExitSpotTo(IntVec3 loc, out bool succeeded)
	{
		int num = 0;
		int num2 = 0;
		IntVec3 intVec2;
		while (true)
		{
			num2++;
			if (num2 > 30)
			{
				succeeded = false;
				return loc;
			}
			bool succeeded2 = false;
			IntVec3 intVec = GenMap.RandomMapSquareNear(loc, num, null, out succeeded2);
			num += 4;
			if (succeeded2)
			{
				int num3 = intVec.x;
				intVec2 = new IntVec3(0, 0, intVec.z);
				if (Find.Map.Size.z - intVec.z < num3)
				{
					num3 = Find.Map.Size.z - intVec.z;
					intVec2 = new IntVec3(intVec.x, 0, Find.Map.Size.z - 1);
				}
				if (Find.Map.Size.x - intVec.x < num3)
				{
					num3 = Find.Map.Size.x - intVec.x;
					intVec2 = new IntVec3(Find.Map.Size.x - 1, 0, intVec.z);
				}
				if (intVec.z < num3)
				{
					intVec2 = new IntVec3(intVec.x, 0, 0);
				}
				if (intVec2.Standable() && Find.ReachabilityRegions.ReachableBetween(loc, new TargetPack(intVec2), adjacentIsOK: false))
				{
					break;
				}
			}
		}
		succeeded = true;
		return intVec2;
	}
}
