using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Incident_AnimalInsanity : IncidentDefinition
{
	public Incident_AnimalInsanity()
	{
		uniqueSaveKey = 21075;
		chance = 2f;
		global = true;
		minRefireInterval = 400000;
		favorability = IncidentFavorability.VeryBad;
		threatLevel = IncidentThreatLevel.BigThreat;
		pointsScaleable = true;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		if (parms.points < 0)
		{
			Debug.LogError("AnimalInsanity running without points.");
			parms.points = (int)(Find.Storyteller.watcher.watcherStrength.StrengthRating * 50f);
		}
		List<RaceDefinition> list = (from kvp in RaceDefDatabase.allRaces
			select kvp.Value into race
			where !race.humanoid && AnimalInsanityUtility.PointsPerAnimal(race) <= (float)parms.points
			select race).ToList();
		list.RemoveAll((RaceDefinition def) => Find.PawnManager.AllPawns.Where((Pawn p) => p.raceDef == def).Count() < 3);
		if (list.Count == 0)
		{
			return false;
		}
		RaceDefinition animalDef = list.RandomElement();
		List<Pawn> list2 = Find.PawnManager.AllPawns.Where((Pawn p) => p.raceDef == animalDef).ToList();
		float num = AnimalInsanityUtility.PointsPerAnimal(animalDef);
		float num2 = 0f;
		int num3 = 0;
		Pawn pawn = null;
		foreach (Pawn item in list2)
		{
			if (num2 + num > (float)parms.points)
			{
				break;
			}
			PsychologyUtility.DoMentalBreak(item, MindBrokenState.Psychotic);
			num2 += num;
			num3++;
			pawn = item;
		}
		if (num3 > 1)
		{
			pawn = null;
		}
		if (num2 == 0f)
		{
			return false;
		}
		string text;
		if (num3 == 1)
		{
			text = "A local " + animalDef.raceName.ToLower() + " has gone mad. It will attack everyone it sees.";
		}
		else
		{
			text = "Some sort of psychic wave has swept over the landscape. Your colonists are okay, but...";
			text += "\n\n";
			text = text + "It seems many of the " + animalDef.raceName.ToLower() + "s in the area has been driven insane.";
		}
		Find.LetterStack.ReceiveLetter(new Letter(text, pawn));
		return true;
	}
}
