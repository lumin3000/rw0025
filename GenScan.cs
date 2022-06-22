using System.Collections;

public static class GenScan
{
	public delegate bool CloseToThingValidator(Thing t);

	public static Thing ClosestReachableThing(IntVec3 searchCenter, IEnumerable searchCollection)
	{
		return ClosestReachableThing(searchCenter, searchCollection, 9999f);
	}

	public static Thing ClosestReachableThing(IntVec3 searchCenter, IEnumerable searchCollection, float maxDistance)
	{
		return ClosestReachableThing(searchCenter, searchCollection, maxDistance, (Thing t) => true);
	}

	public static Thing ClosestReachableThing(IntVec3 searchCenter, IEnumerable searchCollection, CloseToThingValidator validator)
	{
		return ClosestReachableThing(searchCenter, searchCollection, 9999f, validator);
	}

	public static Thing ClosestReachableThing(IntVec3 searchCenter, IEnumerable searchEnum, float maxDistance, CloseToThingValidator validator)
	{
		float num = maxDistance * maxDistance;
		float num2 = 2.1474836E+09f;
		Thing result = null;
		foreach (Thing item in searchEnum)
		{
			float lengthHorizontalSquared = (searchCenter - item.Position).LengthHorizontalSquared;
			if (lengthHorizontalSquared < num2 && lengthHorizontalSquared < num && !item.destroyed && Find.ReachabilityRegions.ReachableBetween(searchCenter, item, adjacentIsOK: true) && validator(item))
			{
				result = item;
				num2 = lengthHorizontalSquared;
			}
		}
		return result;
	}

	public static Thing ClosestThing(IntVec3 searchCenter, IEnumerable searchEnum, float maxDistance, CloseToThingValidator validator)
	{
		float num = maxDistance * maxDistance;
		float num2 = 2.1474836E+09f;
		Thing result = null;
		foreach (Thing item in searchEnum)
		{
			float lengthHorizontalSquared = (searchCenter - item.Position).LengthHorizontalSquared;
			if (lengthHorizontalSquared < num2 && lengthHorizontalSquared < num && !item.destroyed && validator(item))
			{
				result = item;
				num2 = lengthHorizontalSquared;
			}
		}
		return result;
	}
}
