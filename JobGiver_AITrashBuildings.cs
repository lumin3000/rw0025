using System.Collections.Generic;
using UnityEngine;

public class JobGiver_AITrashBuildings : ThinkNode_JobGiver
{
	public float searchRadius = 99999f;

	protected override Job TryGiveTerminalJob()
	{
		//Discarded unreachable code: IL_00a5
		int num = 0;
		List<Building> allBuildingsColonist = Find.BuildingManager.AllBuildingsColonist;
		int count = allBuildingsColonist.Count;
		float num2 = searchRadius * searchRadius;
		Building building;
		do
		{
			int index = Random.Range(0, count);
			building = allBuildingsColonist[index];
			num++;
			if (num > 75)
			{
				return null;
			}
		}
		while (!BuildingTrashUtility.IsGoodTrashTargetFor(building, pawn) || (searchRadius < 9999f && (building.Position - pawn.Position).LengthHorizontalSquared > num2));
		return BuildingTrashUtility.AttackJobOnFor(building, pawn);
	}
}
