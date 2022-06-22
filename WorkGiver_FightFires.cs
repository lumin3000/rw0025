using System.Collections;

internal class WorkGiver_FightFires : WorkGiver
{
	private const int NearbyPawnRadius = 15;

	public override IEnumerable PotentialWorkTargets => Find.ThingLister.spawnedFires;

	public WorkGiver_FightFires(Pawn pawn)
		: base(pawn)
	{
		wType = WorkType.Firefighter;
	}

	public override Job StartingJobOn(Thing t)
	{
		if (t.def.eType != EntityType.Fire)
		{
			return null;
		}
		Pawn pawn = ((AttachableThing)t).parent as Pawn;
		if (pawn != null && pawn.Team == base.pawn.Team)
		{
			if (!Find.HomeZoneGrid.ShouldClean(t.Position) && Gen.ManhattanDistanceFlat(base.pawn.Position, pawn.Position) > 15)
			{
				return null;
			}
		}
		else if (!Find.HomeZoneGrid.ShouldClean(t.Position))
		{
			return null;
		}
		if (!base.pawn.CanReserve(t, ReservationType.Total))
		{
			return null;
		}
		return new Job(JobType.BeatFire, t);
	}
}
