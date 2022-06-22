public class JobGiver_AITrashCloseBuildings : ThinkNode_JobGiver
{
	private const int SearchRadius = 5;

	public float searchRadius = 99999f;

	protected override Job TryGiveTerminalJob()
	{
		IntRect intRect = IntRect.CenteredOn(pawn.Position, 5);
		for (int i = 0; i < 50; i++)
		{
			IntVec3 randomSquare = intRect.RandomSquare;
			if (DebugSettings.drawDestSearch)
			{
				Find.DebugDrawer.MakeDebugSquare(randomSquare);
			}
			if (randomSquare.InBounds())
			{
				Building building = Find.Grids.BlockerAt(randomSquare) as Building;
				if (building != null && BuildingTrashUtility.IsGoodTrashTargetFor(building, pawn) && GenGrid.LineOfSight(pawn.Position, randomSquare))
				{
					return BuildingTrashUtility.AttackJobOnFor(building, pawn);
				}
			}
		}
		return null;
	}
}
