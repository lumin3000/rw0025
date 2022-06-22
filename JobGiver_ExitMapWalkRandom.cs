using UnityEngine;

public class JobGiver_ExitMapWalkRandom : JobGiver_ExitMap
{
	public JobGiver_ExitMapWalkRandom()
	{
		moveSpeed = MoveSpeed.Walk;
	}

	protected override IntVec3 GoodExitDest(out bool succeeded)
	{
		//Discarded unreachable code: IL_00b8
		int num = 0;
		IntVec3 intVec;
		do
		{
			num++;
			if (num > 40)
			{
				succeeded = false;
				return pawn.Position;
			}
			intVec = GenMap.RandomMapSquare();
			int num2 = Random.Range(0, 4);
			if (num2 == 0)
			{
				intVec.x = 0;
			}
			if (num2 == 1)
			{
				intVec.x = Find.Map.Size.x - 1;
			}
			if (num2 == 2)
			{
				intVec.z = 0;
			}
			if (num2 == 3)
			{
				intVec.z = Find.Map.Size.z - 1;
			}
		}
		while (!intVec.Standable() || !pawn.CanReach(intVec));
		succeeded = true;
		return intVec;
	}
}
