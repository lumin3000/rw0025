using System.Collections.Generic;
using System.Linq;

public class Incident_PsychoticAnimal : IncidentDefinition
{
	private const int FixedPoints = 30;

	public Incident_PsychoticAnimal()
	{
		uniqueSaveKey = 20668;
		chance = 9f;
		global = true;
		favorability = IncidentFavorability.Bad;
		threatLevel = IncidentThreatLevel.SmallThreat;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		int maxPoints = 150;
		if (Find.TickManager.tickCount < 200000)
		{
			maxPoints = 40;
		}
		List<Pawn> list = Find.PawnManager.AllPawns.Where((Pawn p) => !p.raceDef.humanoid && AnimalInsanityUtility.PointsPerAnimal(p.raceDef) <= (float)maxPoints).ToList();
		if (list.Count == 0)
		{
			return false;
		}
		Pawn pawn = list.RandomElement();
		PsychologyUtility.DoMentalBreak(pawn, MindBrokenState.Psychotic);
		string text = "A local " + pawn.raceDef.raceName.ToLower() + " has gone mad. It will attack everyone it sees.";
		Find.LetterStack.ReceiveLetter(new Letter(text));
		return true;
	}
}
