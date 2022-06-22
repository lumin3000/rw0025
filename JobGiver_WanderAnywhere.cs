public class JobGiver_WanderAnywhere : JobGiver_Wander
{
	public JobGiver_WanderAnywhere()
	{
		radius = 7f;
		moveSpeed = MoveSpeed.Walk;
		ticksBetweenWandersRange = new IntRange(125, 200);
	}

	protected override IntVec3 GetWanderRoot()
	{
		return pawn.Position;
	}
}
