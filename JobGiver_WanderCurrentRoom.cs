public class JobGiver_WanderCurrentRoom : JobGiver_Wander
{
	public JobGiver_WanderCurrentRoom()
	{
		radius = 7f;
		ticksBetweenWandersRange = new IntRange(125, 200);
		moveSpeed = MoveSpeed.Amble;
	}

	public override void SetPawn(Pawn newPawn)
	{
		base.SetPawn(newPawn);
		wanderDestValidator = (IntVec3 loc) => WanderUtility.InSameRoom(newPawn.Position, loc);
	}

	protected override IntVec3 GetWanderRoot()
	{
		return pawn.Position;
	}
}
