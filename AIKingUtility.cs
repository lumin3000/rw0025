using System.Collections.Generic;
using System.Linq;

public static class AIKingUtility
{
	public static AIKing GetKing(this Pawn p)
	{
		return Find.AIKingManager.KingOf(p);
	}

	public static IntVec3 GoodDropSpot()
	{
		//Discarded unreachable code: IL_00ef
		int num = 0;
		float num2 = 65f;
		IntVec3 intVec;
		while (true)
		{
			intVec = GenMap.RandomMapSquare();
			num++;
			if (!intVec.Standable() || Find.RoofGrid.Roofed(intVec))
			{
				continue;
			}
			if (num > 300)
			{
				return intVec;
			}
			num2 -= 0.2f;
			IEnumerable<Thing> enumerable = Find.PawnManager.Colonists.Cast<Thing>().Concat(Find.BuildingManager.AllBuildingsColonistCombatTargets.Cast<Thing>());
			bool flag = false;
			foreach (Thing item in enumerable)
			{
				if ((intVec - item.Position).LengthHorizontalSquared < num2 * num2)
				{
					flag = true;
					break;
				}
			}
			if (!flag && !intVec.Isolated())
			{
				break;
			}
		}
		return intVec;
	}
}
