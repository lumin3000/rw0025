using UnityEngine;

public abstract class JobGiver_AIFight : ThinkNode_JobGiver
{
	private const int TicksToWaitAtDestinationMin = 1000;

	private const int TicksToWaitAtDestinationMax = 1250;

	private const int TicksToMoveBeforeReevalMin = 150;

	private const int TicksToMoveBeforeReevalMax = 175;

	private const int PositionReThinkDelay = 100;

	private Thing EnemyTarget
	{
		get
		{
			return pawn.MindState.enemyTarget;
		}
		set
		{
			pawn.MindState.enemyTarget = value;
		}
	}

	protected abstract GenAI.AIDestination NewFightDestination();

	protected override Job TryGiveTerminalJob()
	{
		if (EnemyTarget == null || pawn.equipment.Primary == null)
		{
			return null;
		}
		bool flag = CoverUtility.CoverGiverSetAtFrom(pawn.Position, EnemyTarget.Position).overallBlockChance > 0.01f;
		bool flag2 = pawn.Position.Standable();
		bool flag3 = pawn.equipment.Primary.verb.CanHitTarget(new TargetPack(EnemyTarget));
		if (flag && flag2 && flag3)
		{
			Job job = new Job(JobType.Wait);
			job.TimeLimit = Random.Range(1000, 1251);
			return job;
		}
		GenAI.AIDestination aIDestination = NewFightDestination();
		if (!aIDestination.found || aIDestination.pos == pawn.Position)
		{
			Job job2 = new Job(JobType.Wait);
			job2.TimeLimit = 100;
			return job2;
		}
		Find.PawnDestinationManager.ReserveDestinationFor(pawn, aIDestination.pos);
		Job job3 = new Job(JobType.Goto, new TargetPack(aIDestination.pos));
		job3.TimeLimit = Random.Range(150, 176);
		return job3;
	}
}
