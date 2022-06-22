using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn_WorkSettings : Saveable
{
	public const int MaxPriority = 4;

	private Pawn pawn;

	private Dictionary<WorkType, int> workPriorities = new Dictionary<WorkType, int>();

	public IEnumerable<WorkType> ActiveWorksByPriority
	{
		get
		{
			if (Find.PlaySettings.useWorkPriorities)
			{
				return from kvp in workPriorities
					where kvp.Value > 0
					orderby kvp.Value, -kvp.Key.GetDefinition().naturalPriority
					select kvp.Key;
			}
			return from kvp in workPriorities
				where kvp.Value > 0
				orderby -kvp.Key.GetDefinition().naturalPriority
				select kvp.Key;
		}
	}

	public Pawn_WorkSettings()
	{
	}

	public Pawn_WorkSettings(Pawn pawn)
	{
		this.pawn = pawn;
		IEnumerable<WorkType> enumerable = from def in WorkDefDatabase.AutomaticWorksInPriorityOrder
			where def.startActive
			select def.wType;
		foreach (WorkType item in enumerable)
		{
			workPriorities.Add(item, 4);
		}
	}

	public void ApplyWorkDisables()
	{
		foreach (WorkType disabledWork in pawn.story.DisabledWorks)
		{
			SetWorkToPriority(disabledWork, 0);
		}
	}

	public void ExposeData()
	{
		Scribe.LookDict(ref workPriorities, "WorkPriorities");
	}

	public void SetWorkToPriority(WorkType w, int priority)
	{
		if (priority != 0 && pawn.story.WorkIsDisabled(w))
		{
			Debug.LogError(string.Concat("Tried to change priority on disabled worktype ", w, " for pawn ", pawn));
			return;
		}
		if (priority < 0 || priority > 4)
		{
			Debug.Log("Trying to set work to invalid priority " + priority);
		}
		if (!workPriorities.ContainsKey(w))
		{
			workPriorities.Add(w, priority);
		}
		else
		{
			workPriorities[w] = priority;
		}
		if (priority == 0)
		{
			pawn.MindState.Notify_WorkPriorityDisabled(w);
		}
	}

	public int GetPriorityOf(WorkType w)
	{
		if (!workPriorities.ContainsKey(w))
		{
			return 0;
		}
		int num = workPriorities[w];
		if (num > 0 && !Find.PlaySettings.useWorkPriorities)
		{
			return 1;
		}
		return num;
	}
}
