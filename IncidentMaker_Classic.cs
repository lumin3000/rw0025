using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class IncidentMaker_Classic : IncidentMaker
{
	private const float RandomEventChance = 0.17f;

	private const int MinIntervalBigThreats = 40000;

	private const float BigThreatChance = 0.4f;

	private const float SmallThreatChance = 0.06f;

	private const float MaxDesiredPopulation = 7f;

	protected float challengeScale;

	private int IntervalsPassed => Find.TickManager.tickCount / 5000;

	protected override IEnumerable<QueuedIncident> NewIncidentSet()
	{
		if (UnityEngine.Random.value < 0.17f && DateHandler.DaysPassed > 2)
		{
			List<IncidentDefinition> allowedRandomIncidents = IncidentDatabase.GlobalQueueableIncidents.ToList();
			allowedRandomIncidents.RemoveAll((IncidentDefinition inc) => inc.threatLevel > IncidentThreatLevel.NoThreat);
			foreach (IncidentDefinition inc2 in allowedRandomIncidents.ListFullCopy())
			{
				if (UnityEngine.Random.value > Find.Storyteller.intenderPopulation.PopulationIntent)
				{
					allowedRandomIncidents.Remove(inc2);
				}
			}
			IncidentDefinition selectedDef = allowedRandomIncidents.RandomElementByWeight((IncidentDefinition e) => e.chance);
			yield return new QueuedIncident(selectedDef);
		}
		Func<IncidentDefinition, QueuedIncident> newQueuedIncident = delegate(IncidentDefinition inc)
		{
			QueuedIncident queuedIncident = new QueuedIncident(inc);
			if (queuedIncident.def.pointsScaleable)
			{
				queuedIncident.parms.points = PointsForIncidentNow(queuedIncident.def);
			}
			return queuedIncident;
		};
		if (IntervalsPassed == 18)
		{
			yield return newQueuedIncident(IncidentDatabase.RandomSmallThreat());
		}
		if (IntervalsPassed == 25)
		{
			yield return new QueuedIncident(IncidentDatabase.RandomBigThreat())
			{
				parms = 
				{
					points = 35,
					forceIncap = true
				}
			};
		}
		if (IntervalsPassed == 38)
		{
			QueuedIncident raidTwo = new QueuedIncident(IncidentDatabase.RandomBigThreat());
			raidTwo.parms.points = Mathf.RoundToInt((float)PointsForIncidentNow(raidTwo.def) * 0.5f);
			yield return raidTwo;
		}
		if (IntervalsPassed > 52 && IntervalsPassed / 4 % 10 > 5)
		{
			if ((float)Find.TickManager.tickCount > base.StoryState.lastThreatQueueTime + 40000f && UnityEngine.Random.value < 0.4f)
			{
				yield return newQueuedIncident(IncidentDatabase.IncidentDefOfType(typeof(Incident_Raid)));
			}
			if (UnityEngine.Random.value < 0.06f)
			{
				yield return newQueuedIncident(IncidentDatabase.RandomSmallThreat());
			}
		}
	}

	public override int PointsForIncidentNow(IncidentDefinition def)
	{
		if (def.threatLevel == IncidentThreatLevel.NoThreat)
		{
			Debug.LogError(string.Concat("Incident ", def, " wants points but I don't understand what to give it."));
			return 100;
		}
		float f;
		if (base.StoryState.numThreatsQueued == 0)
		{
			f = 40f;
		}
		else
		{
			float num = IncidentMakerUtility.StrengthAdjustedThreatPointsNow;
			float value = IncidentMakerUtility.TimeAdjustedThreatPointsNow;
			f = Mathf.Clamp(value, num, num + 50f);
			f *= challengeScale;
		}
		return Mathf.RoundToInt(f);
	}
}
