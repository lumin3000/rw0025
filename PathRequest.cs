public class PathRequest
{
	public Pawn pawn;

	public IntVec3 start;

	public TargetPack dest;

	public PathingParameters pathParams;

	public bool adjacentIsOK;

	public override string ToString()
	{
		return string.Concat(start, " to ", dest);
	}
}
