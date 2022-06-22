using System.Text;
using UnityEngine;

public class Incident_SolarFlare : IncidentDefinition
{
	public Incident_SolarFlare()
	{
		uniqueSaveKey = 71453;
		chance = 2.5f;
		global = true;
		minRefireInterval = 100000;
		favorability = IncidentFavorability.Bad;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		if (Find.MapConditionManager.ConditionIsActive(MapConditionType.SolarFlare))
		{
			return false;
		}
		int ticksToExpire = Random.Range(12000, 28000);
		Find.MapConditionManager.RegisterCondition(new MapCondition(MapConditionType.SolarFlare, ticksToExpire));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("A solar flare has begun.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("The intense radiation will shut down all electrical devices.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("It should pass in about a day.");
		Find.LetterStack.ReceiveLetter(new Letter(stringBuilder.ToString()));
		return true;
	}
}
