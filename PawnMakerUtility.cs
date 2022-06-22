public static class PawnMakerUtility
{
	public static void AddOrRemovePawnTrackersFor(Pawn pawn)
	{
		if (pawn.Team == TeamType.Prisoner)
		{
			if (pawn.prisoner == null)
			{
				pawn.prisoner = new Pawn_PrisonerTracker(pawn);
			}
		}
		else
		{
			pawn.prisoner = null;
		}
	}

	public static void GiveAppropriateKeysTo(Pawn p)
	{
		if (p.Team == TeamType.Colonist && !p.inventory.Has(EntityType.DoorKey))
		{
			p.inventory.Take(ThingMaker.MakeThing(EntityType.DoorKey));
		}
	}
}
