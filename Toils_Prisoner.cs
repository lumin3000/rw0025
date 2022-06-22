public static class Toils_Prisoner
{
	public static Toil GotoPrisoner(Pawn pawn, Pawn talkee)
	{
		Toil toil = new Toil();
		toil.initAction = delegate
		{
			pawn.pather.StartPathTowards(talkee);
		};
		toil.tickFailCondition = delegate
		{
			if (!ToilTools.CanInteractStandard(pawn, talkee))
			{
				return true;
			}
			if (talkee.Incapacitated)
			{
				return true;
			}
			if (talkee.IsInBed())
			{
				return true;
			}
			return (talkee.Team != TeamType.Prisoner || talkee.prisoner.interactionMode == PrisonerInteractionMode.NoInteraction) ? true : false;
		};
		toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
		return toil;
	}

	public static Toil TryRecruitPrisoner(Pawn jobDoer, Pawn prisoner)
	{
		Toil toil = new Toil();
		toil.initAction = delegate
		{
			if (!prisoner.destroyed && !prisoner.Incapacitated && prisoner.Team == TeamType.Prisoner && prisoner.prisoner.tryRecruit)
			{
				prisoner.prisoner.lastWardenVisitTime = Find.TickManager.tickCount;
				SpeechConfig speechConfig = new SpeechConfig();
				speechConfig.specialEffect = SpeechEffect.TryRecruit;
				speechConfig.TrySendFromTo(jobDoer, prisoner);
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Delay;
		toil.duration = 100;
		return toil;
	}
}
