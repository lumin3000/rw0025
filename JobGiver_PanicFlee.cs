public class JobGiver_PanicFlee : JobGiver_ExitMap
{
	public JobGiver_PanicFlee()
	{
		moveSpeed = MoveSpeed.Jog;
	}

	protected override IntVec3 GoodExitDest(out bool succeeded)
	{
		return ExitUtility.ClosestExitSpotTo(pawn.Position, out succeeded);
	}
}
