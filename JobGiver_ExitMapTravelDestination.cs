public class JobGiver_ExitMapTravelDestination : JobGiver_ExitMap
{
	public JobGiver_ExitMapTravelDestination()
	{
		moveSpeed = MoveSpeed.Walk;
	}

	protected override IntVec3 GoodExitDest(out bool succeeded)
	{
		succeeded = true;
		return pawn.MindState.travelDestination;
	}
}
