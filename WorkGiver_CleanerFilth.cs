using System.Collections;

internal class WorkGiver_CleanerFilth : WorkGiver
{
	public override IEnumerable PotentialWorkTargets => Find.HomeZoneGrid.AllCleanableFilth;

	public WorkGiver_CleanerFilth(Pawn pawn)
		: base(pawn)
	{
		wType = WorkType.Cleaning;
	}

	public override Job StartingJobOn(Thing t)
	{
		if (t.def.category != EntityCategory.Filth)
		{
			return null;
		}
		if (!Find.HomeZoneGrid.ShouldClean(t.Position))
		{
			return null;
		}
		if (!pawn.CanReserve(t, ReservationType.Total))
		{
			return null;
		}
		return new Job(JobType.Clean, t);
	}
}
