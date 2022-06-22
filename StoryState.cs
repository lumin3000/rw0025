using System.Collections.Generic;
using UnityEngine;

public class StoryState : Saveable
{
	public int numThreatsQueued;

	public float lastThreatQueueTime = -1f;

	private Dictionary<int, int> lastQueueTicks = new Dictionary<int, int>();

	public void ExposeData()
	{
		Scribe.LookField(ref lastThreatQueueTime, "LastThreatQueueTime", forceSave: true);
		Scribe.LookField(ref numThreatsQueued, "NumThreatsQueued", forceSave: true);
		Scribe.LookDict(ref lastQueueTicks, "LastQueueTicks");
	}

	public bool AllowNewQueuedIncident(QueuedIncident qi)
	{
		if (qi == null)
		{
			Debug.LogError("Cannot check AllowNewQueuedIncident on a null qi.");
			return false;
		}
		if (qi.def == null)
		{
			Debug.LogError("qi has no def! Wtf!");
			return false;
		}
		int uniqueSaveKey = qi.def.uniqueSaveKey;
		if (lastQueueTicks.TryGetValue(uniqueSaveKey, out var value))
		{
			return qi.occurTick >= value + qi.def.minRefireInterval;
		}
		return true;
	}

	public void RecordNewQueuedIncident(QueuedIncident qi)
	{
		if (qi.def.threatLevel == IncidentThreatLevel.BigThreat)
		{
			if (lastThreatQueueTime <= (float)qi.occurTick)
			{
				lastThreatQueueTime = qi.occurTick;
			}
			else
			{
				Debug.LogError(string.Concat("Queueing threats backwards in time (", qi, ")"));
			}
			numThreatsQueued++;
		}
		if (lastQueueTicks.ContainsKey(qi.def.uniqueSaveKey))
		{
			lastQueueTicks[qi.def.uniqueSaveKey] = qi.occurTick;
		}
		else
		{
			lastQueueTicks.Add(qi.def.uniqueSaveKey, qi.occurTick);
		}
	}
}
