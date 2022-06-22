public class JobGiver_AIDefendPoint : JobGiver_AIFight
{
	private const float MaxRangeFromHome = 8f;

	protected override GenAI.AIDestination NewFightDestination()
	{
		CastingPositionRequest castingPositionRequest = new CastingPositionRequest();
		castingPositionRequest.moverPawn = pawn;
		castingPositionRequest.targetThing = pawn.MindState.enemyTarget;
		castingPositionRequest.defendHome = pawn.MindState.dutyLocation;
		castingPositionRequest.maxRangeFromDefendHome = 8f;
		return CastPositionFinder.FindCastingPosition(castingPositionRequest);
	}
}
