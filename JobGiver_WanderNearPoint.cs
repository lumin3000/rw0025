public class JobGiver_WanderNearPoint : JobGiver_Wander
{
	public JobGiver_WanderNearPoint()
	{
		radius = 7f;
		ticksBetweenWandersRange = new IntRange(125, 200);
	}

	protected override IntVec3 GetWanderRoot()
	{
		return pawn.MindState.dutyLocation;
	}
}
