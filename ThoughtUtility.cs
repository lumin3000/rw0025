public static class ThoughtUtility
{
	public static void BroadcastThought(ThoughtType ThType, IntVec3 Root, float Radius)
	{
		foreach (Pawn colonist in Find.PawnManager.Colonists)
		{
			if (colonist.Position.WithinHorizontalDistanceOf(Root, Radius) && GenGrid.LineOfSight(colonist.Position, Root) && !colonist.IsInBed())
			{
				colonist.psychology.thoughts.GainThought(ThType);
			}
		}
	}
}
