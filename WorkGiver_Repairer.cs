using System.Collections;

public class WorkGiver_Repairer : WorkGiver
{
	public override IEnumerable PotentialWorkTargets => Find.BuildingManager.AllBuildingsColonist;

	public WorkGiver_Repairer(Pawn newPawn)
		: base(newPawn)
	{
		wType = WorkType.Repair;
	}

	public override Job StartingJobOn(Thing t)
	{
		if (t.Team != TeamType.Colonist)
		{
			return null;
		}
		Building building = t as Building;
		if (building != null)
		{
			if (building.RepairUrgency == 0)
			{
				return null;
			}
			if (!pawn.CanReserve(building, ReservationType.Construction))
			{
				return null;
			}
			if (building.IsBurningImmobile())
			{
				return null;
			}
			return new Job(JobType.Repair, new TargetPack(t));
		}
		return null;
	}
}
