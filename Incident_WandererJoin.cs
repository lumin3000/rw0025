using System.Collections.Generic;

public class Incident_WandererJoin : IncidentDefinition
{
	public Incident_WandererJoin()
	{
		uniqueSaveKey = 61129;
		chance = 0.5f;
		global = true;
		favorability = IncidentFavorability.VeryGood;
		populationEffect = IncidentPopulationEffect.Increase;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		IntVec3 newThingPos = GenMap.RandomEdgeSquareWith((IntVec3 sq) => !sq.Isolated());
		List<string> list = new List<string>();
		list.Add("Traveler");
		list.Add("Traveler");
		list.Add("Drifter");
		list.Add("Refugee");
		string text = list.RandomElement();
		Pawn pawn = PawnMaker.GeneratePawn(text, TeamType.Colonist);
		ThingMaker.Spawn(pawn, newThingPos);
		string baseText = "A " + text.ToLower() + " named NAME has arrived and is joining the colony. HECAP is a " + pawn.story.Adulthood.title.ToLower() + ".";
		baseText = GenText.TextAdjustedFor(pawn, baseText);
		Find.LetterStack.ReceiveLetter(new Letter(baseText, pawn));
		Find.Storyteller.intenderPopulation.Notify_PopulationGainIncident();
		return true;
	}
}
