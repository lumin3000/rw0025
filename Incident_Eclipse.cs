using UnityEngine;

internal class Incident_Eclipse : IncidentDefinition
{
	public Incident_Eclipse()
	{
		uniqueSaveKey = 2768;
		chance = 5f;
		global = true;
		minRefireInterval = 100000;
		favorability = IncidentFavorability.Bad;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		if (Find.MapConditionManager.ConditionIsActive(MapConditionType.Eclipse))
		{
			return false;
		}
		int ticksToExpire = Mathf.RoundToInt(Random.Range(1.5f, 2.5f) * 20000f);
		Find.MapConditionManager.RegisterCondition(new MapCondition_Eclipse(ticksToExpire));
		string text = "This moon has orbited behind the gas giant. An eclipse has begun.";
		Find.LetterStack.ReceiveLetter(new Letter(text));
		return true;
	}
}
