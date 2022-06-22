using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class IncidentQueue : Saveable
{
	private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();

	public int Count => queuedIncidents.Count;

	public string DebugQueueReadout
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (QueuedIncident queuedIncident in queuedIncidents)
			{
				stringBuilder.AppendLine(queuedIncident.ToString() + " (in " + (queuedIncident.occurTick - Find.TickManager.tickCount) + " ticks)");
			}
			return stringBuilder.ToString();
		}
	}

	public IEnumerator GetEnumerator()
	{
		foreach (QueuedIncident queuedIncident in queuedIncidents)
		{
			yield return queuedIncident;
		}
	}

	public void Clear()
	{
		queuedIncidents.Clear();
	}

	public void ExposeData()
	{
		Scribe.LookList(ref queuedIncidents, "QueuedIncidents");
	}

	public void AddQueuedIncident(QueuedIncident qi)
	{
		if (qi.parms.points > 0 && !qi.def.pointsScaleable)
		{
			Debug.LogError(string.Concat("Points scaling ", qi.def, " to ", qi.parms.points, " but it is not points scaleable."));
		}
		if (Find.Storyteller.storyState.AllowNewQueuedIncident(qi))
		{
			Find.Storyteller.storyState.RecordNewQueuedIncident(qi);
			queuedIncidents.Add(qi);
			queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.occurTick.CompareTo(b.occurTick));
		}
	}

	public void IncidentManagerTick()
	{
		foreach (QueuedIncident queuedIncident in queuedIncidents)
		{
			if (queuedIncident.occurTick == Find.TickManager.tickCount)
			{
				queuedIncident.def.TryExecute(queuedIncident.parms);
			}
		}
		queuedIncidents.RemoveAll((QueuedIncident qi) => Find.TickManager.tickCount >= qi.occurTick);
	}
}
