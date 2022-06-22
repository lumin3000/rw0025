public static class PawnLocalAwareness
{
	private const float SightRadius = 30f;

	public static bool AwareOf(this Pawn p, Thing t)
	{
		if (p.raceDef.GloballyAware)
		{
			return true;
		}
		if ((p.Position - t.Position).LengthHorizontalSquared > 900f)
		{
			return false;
		}
		if (p.ContainingRoom() != t.ContainingRoom())
		{
			return false;
		}
		if (!GenGrid.LineOfSight(p.Position, t.Position))
		{
			return false;
		}
		return true;
	}
}
