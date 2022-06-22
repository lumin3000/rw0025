using UnityEngine;

public class JobGiver_Psychotic : ThinkNode_JobGiver
{
	private const float MaxAttackDistance_Humanoid = 15f;

	private const float WaitChance = 0.5f;

	private const int WaitTicks = 90;

	private const int MinMeleeChaseTicks = 420;

	private const int MaxMeleeChaseTicks = 900;

	protected override Job TryGiveTerminalJob()
	{
		if (Random.value < 0.5f)
		{
			Job job = new Job(JobType.Wait);
			job.TimeLimit = 90;
			return job;
		}
		GenScan.CloseToThingValidator validator = delegate(Thing t)
		{
			if (t == base.pawn)
			{
				return false;
			}
			Pawn pawn2 = t as Pawn;
			if (pawn2.Incapacitated)
			{
				return false;
			}
			if (!base.pawn.raceDef.humanoid && !pawn2.raceDef.humanoid)
			{
				return false;
			}
			return (!base.pawn.raceDef.humanoid || GenGrid.LineOfSight(base.pawn.Position, pawn2.Position)) ? true : false;
		};
		float maxDistance = ((!base.pawn.raceDef.humanoid) ? 9999f : 15f);
		Pawn pawn = (Pawn)GenScan.ClosestReachableThing(base.pawn.Position, Find.PawnManager.AllPawns, maxDistance, validator);
		if (pawn != null)
		{
			Job job2 = new Job(JobType.AttackMelee, pawn);
			job2.maxNumMeleeAttacks = 1;
			job2.TimeLimit = Random.Range(420, 900);
			return job2;
		}
		return null;
	}
}
