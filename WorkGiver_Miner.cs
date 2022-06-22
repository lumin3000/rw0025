using System.Collections;

public class WorkGiver_Miner : WorkGiver
{
	public override IEnumerable PotentialWorkTargets => Find.DesignationManager.ShouldMineDesignatedThings;

	public WorkGiver_Miner(Pawn pawn)
		: base(pawn)
	{
		wType = WorkType.Mining;
	}

	public override Job StartingJobOn(Thing t)
	{
		if (!t.def.mineable)
		{
			return null;
		}
		if (!pawn.CanReserve(t, ReservationType.Total))
		{
			return null;
		}
		GenPath.SpotToStandAdjacentToFor(pawn, t, out var succeeded);
		if (!succeeded)
		{
			return null;
		}
		if (Find.DesignationManager.DesignationAt(t.Position, DesignationType.Mine) == null)
		{
			return null;
		}
		return new Job(JobType.Mine, new TargetPack(t));
	}
}
