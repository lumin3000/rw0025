public class JobGiver_RunRandom : JobGiver_Wander
{
	public JobGiver_RunRandom()
	{
		radius = 7f;
		ticksBetweenWandersRange = new IntRange(5, 10);
		moveSpeed = MoveSpeed.Sprint;
	}

	protected override IntVec3 GetWanderRoot()
	{
		return pawn.Position;
	}
}
