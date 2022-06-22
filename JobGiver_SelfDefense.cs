public class JobGiver_SelfDefense : ThinkNode_JobGiver
{
	private const float MaxAttackDistance = 3f;

	private const int HarmForgetTime = 400;

	private const int MaxMeleeChaseTicks = 200;

	private Pawn CloseThreat
	{
		get
		{
			return pawn.MindState.closeThreat;
		}
		set
		{
			pawn.MindState.closeThreat = value;
		}
	}

	protected override Job TryGiveTerminalJob()
	{
		if (CloseThreat == null)
		{
			return null;
		}
		if (CloseThreat.destroyed || CloseThreat.Incapacitated || pawn.MindState.lastCloseThreatHarmTime - Find.TickManager.tickCount > 400 || (pawn.Position - CloseThreat.Position).LengthHorizontalSquared > 9f || !GenGrid.LineOfSight(pawn.Position, CloseThreat.Position))
		{
			CloseThreat = null;
			return null;
		}
		Job job = new Job(JobType.AttackMelee, CloseThreat);
		job.maxNumMeleeAttacks = 1;
		job.TimeLimit = 200;
		return job;
	}
}
