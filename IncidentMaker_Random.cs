using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IncidentMaker_Random : IncidentMaker
{
	private float BigThreatRemoveChance = 0.25f;

	protected override IEnumerable<QueuedIncident> NewIncidentSet()
	{
		if (Find.TickManager.tickCount < 20000)
		{
			yield break;
		}
		List<IncidentDefinition> allowedIncidents = IncidentDatabase.GlobalQueueableIncidents.ToList();
		if (Random.value < BigThreatRemoveChance)
		{
			allowedIncidents.RemoveAll((IncidentDefinition inc) => inc.threatLevel == IncidentThreatLevel.BigThreat);
		}
		int numRandoms = Random.Range(0, 2);
		for (int i = 0; i < numRandoms; i++)
		{
			IncidentDefinition selectedDef = allowedIncidents.RandomElementByWeight((IncidentDefinition e) => e.chance);
			QueuedIncident queuedInc = new QueuedIncident(selectedDef);
			if (selectedDef.pointsScaleable)
			{
				queuedInc.parms.points = PointsForIncidentNow(selectedDef);
			}
			yield return queuedInc;
		}
	}

	public override int PointsForIncidentNow(IncidentDefinition def)
	{
		if (def.threatLevel == IncidentThreatLevel.NoThreat)
		{
			Debug.LogError(string.Concat("Incident ", def, " wants points but I don't understand what to give it."));
			return 100;
		}
		int strengthAdjustedThreatPointsNow = IncidentMakerUtility.StrengthAdjustedThreatPointsNow;
		return Mathf.RoundToInt((float)strengthAdjustedThreatPointsNow * Random.Range(0.4f, 1.7f));
	}
}
