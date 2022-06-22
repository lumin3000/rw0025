public class Incident_RefugeePodCrash : IncidentDefinition
{
	private const float FogClearRadius = 4.5f;

	public Incident_RefugeePodCrash()
	{
		uniqueSaveKey = 512318;
		chance = 1.5f;
		global = true;
		favorability = IncidentFavorability.Good;
		populationEffect = IncidentPopulationEffect.Increase;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		IntVec3 intVec = GenMap.RandomSquareWith((IntVec3 sq) => sq.Standable() && !sq.IsFogged());
		Find.LetterStack.ReceiveLetter(new Letter("You've detected an escape pod crashing hard nearby.\n\nIf anyone survived the impact, they'll be badly wounded.", intVec));
		Pawn pawn = PawnMaker.GeneratePawn("Refugee", TeamType.Traveler);
		pawn.healthTracker.ForceIncap();
		DropPodContentsInfo contents = new DropPodContentsInfo(pawn);
		DropPodUtility.MakeDropPodAt(intVec, contents);
		Find.FogGrid.ClearFogCircle(intVec, 4.5f);
		Find.Storyteller.intenderPopulation.Notify_PopulationGainIncident();
		return true;
	}
}
