using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class IncidentMaker
{
	public const int QueueInterval = 5000;

	protected const int IntervalsPerDay = 4;

	protected IncidentQueue IncQueue => Find.Storyteller.incidentQueue;

	protected StoryState StoryState => Find.Storyteller.storyState;

	public void IncidentMakerTick()
	{
		if (Find.TickManager.tickCount % 5000 != 5)
		{
			return;
		}
		if (IncQueue.Count > 0)
		{
			Debug.LogError("Spillover incidents:");
			foreach (object item in IncQueue)
			{
				Debug.Log("    " + item);
			}
			IncQueue.Clear();
		}
		foreach (QueuedIncident item2 in NewIncidentSet())
		{
			IncQueue.AddQueuedIncident(item2);
		}
	}

	protected abstract IEnumerable<QueuedIncident> NewIncidentSet();

	public void DebugLogTestFutureIncidents()
	{
		StoryState storyState = Find.Storyteller.storyState;
		int tickCount = Find.TickManager.tickCount;
		Find.Storyteller.storyState = new StoryState();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Test story events:");
		for (int i = 0; i < 240; i++)
		{
			foreach (QueuedIncident item in NewIncidentSet())
			{
				if (!Find.Storyteller.storyState.AllowNewQueuedIncident(item))
				{
					stringBuilder.AppendLine("    DISALLOWED " + item);
					continue;
				}
				Find.Storyteller.storyState.RecordNewQueuedIncident(item);
				stringBuilder.AppendLine("    " + item);
			}
			Find.TickManager.tickCount += 5000;
		}
		Debug.Log(stringBuilder.ToString());
		Find.TickManager.tickCount = tickCount;
		Find.Storyteller.storyState = storyState;
	}

	public abstract int PointsForIncidentNow(IncidentDefinition inc);

	public virtual void ExposeData()
	{
	}
}
