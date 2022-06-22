using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IncidentMaker_Friendly : IncidentMaker
{
	private const int MinTicksBetweenThreats = 100000;

	private const float MaxDesiredPopulation = 15f;

	private const float ThreatChance = 0.05f;

	protected override IEnumerable<QueuedIncident> NewIncidentSet()
	{
		if (Find.TickManager.tickCount < 20000)
		{
			yield break;
		}
		List<QueuedIncident> incList = new List<QueuedIncident>();
		List<IncidentDefinition> allowedIncidents = IncidentDatabase.GlobalQueueableIncidents.ToList();
		allowedIncidents.RemoveAll((IncidentDefinition inc) => inc.favorability < IncidentFavorability.Neutral);
		if (Find.Storyteller.intenderPopulation.AdjustedPopulation > 15f)
		{
			allowedIncidents.RemoveAll((IncidentDefinition inc) => inc.populationEffect > IncidentPopulationEffect.None);
		}
		float selector = Random.value;
		int numIncidents = ((!(selector < 0.75f)) ? 1 : 0);
		for (int i = 0; i < numIncidents; i++)
		{
			IncidentDefinition incident = allowedIncidents.RandomElementByWeight((IncidentDefinition def) => def.chance);
			yield return new QueuedIncident(incident);
		}
		if ((float)Find.TickManager.tickCount > base.StoryState.lastThreatQueueTime + 100000f && Random.value < 0.05f)
		{
			QueuedIncident qi = new QueuedIncident(IncidentDatabase.RandomBigThreat());
			qi.parms.points = PointsForIncidentNow(qi.def);
			yield return qi;
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
		strengthAdjustedThreatPointsNow = Mathf.RoundToInt((float)strengthAdjustedThreatPointsNow * 0.25f);
		if (strengthAdjustedThreatPointsNow > 120)
		{
			strengthAdjustedThreatPointsNow = 120;
		}
		return strengthAdjustedThreatPointsNow;
	}
}
