public class JobGiver_GetRest : ThinkNode_JobGiver
{
	protected override Job TryGiveTerminalJob()
	{
		Building_Bed building_Bed = BedUtility.FindBedFor(pawn);
		if (building_Bed != null)
		{
			if (building_Bed != null && building_Bed.owner != pawn)
			{
				pawn.ownership.ClaimBed(building_Bed);
			}
			return new Job(JobType.Sleep, pawn.ownership.ownedBed);
		}
		return null;
	}
}
