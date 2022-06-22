public class CastingPositionRequest
{
	public Pawn moverPawn;

	public Thing targetThing;

	public float maxRangeFromMover = 9999f;

	public float maxRangeFromTarget = 9999f;

	public float maxRangeFromDefendHome = 9999f;

	public IntVec3 defendHome = new IntVec3(-1337, 0, 0);

	public bool wantCoverFromTarget = true;
}
