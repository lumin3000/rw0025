public static class SocialProperness
{
	public static bool IsSociallyProperForUseBy(this Thing t, Pawn p)
	{
		return t.IsSociallyProperForUseBy(p, p.Team);
	}

	public static bool IsSociallyProperForUseBy(this Thing t, Pawn p, TeamType pTeam)
	{
		if (pTeam == TeamType.Prisoner && p.ContainingRoom() != t.ContainingRoom())
		{
			return false;
		}
		if (pTeam == TeamType.Colonist && t.Position.IsInPrisonCell())
		{
			return false;
		}
		return true;
	}

	public static bool IsSociallyProperDestinationFor(this IntVec3 Dest, Pawn p)
	{
		if (p.Team == TeamType.Prisoner && !Dest.IsInPrisonCell())
		{
			return false;
		}
		if (p.Team == TeamType.Colonist && Dest.IsInPrisonCell())
		{
			return false;
		}
		return true;
	}
}
