public class JobGiver_AIAttackTarget : JobGiver_AIFight
{
	protected override GenAI.AIDestination NewFightDestination()
	{
		CastingPositionRequest castingPositionRequest = new CastingPositionRequest();
		castingPositionRequest.moverPawn = pawn;
		castingPositionRequest.targetThing = pawn.MindState.enemyTarget;
		castingPositionRequest.maxRangeFromTarget = pawn.equipment.Primary.verb.VerbDef.range;
		return CastPositionFinder.FindCastingPosition(castingPositionRequest);
	}
}
