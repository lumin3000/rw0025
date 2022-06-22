using System.Collections.Generic;
using System.Linq;

public class JobGiver_WanderColony : JobGiver_Wander
{
	public JobGiver_WanderColony()
	{
		radius = 7f;
		ticksBetweenWandersRange = new IntRange(125, 200);
		wanderDestValidator = (IntVec3 loc) => (!loc.IsInPrisonCell()) ? true : false;
	}

	protected override IntVec3 GetWanderRoot()
	{
		//Discarded unreachable code: IL_00b4
		List<Building> list = Find.BuildingManager.AllBuildingsColonist.ToList();
		if (list.Count == 0)
		{
			return pawn.Position;
		}
		int num = 0;
		IntVec3 intVec;
		while (true)
		{
			num++;
			if (num > 20)
			{
				return pawn.Position;
			}
			Building building = list.RandomElement();
			int num2 = 15 + num * 2;
			if (!((pawn.Position - building.Position).LengthHorizontalSquared > (float)(num2 * num2)))
			{
				intVec = Gen.AdjacentSquares8Way(building).ToList().RandomElement();
				if (intVec.Standable() && pawn.CanReach(intVec))
				{
					break;
				}
			}
		}
		return intVec;
	}
}
