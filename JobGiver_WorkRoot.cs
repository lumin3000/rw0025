using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobGiver_WorkRoot : ThinkNode_JobGiver
{
	public bool emergencyWork;

	private WorkType lastGivenWorkType;

	private List<WorkGiver> workGivers = new List<WorkGiver>();

	public override void SetPawn(Pawn newPawn)
	{
		base.SetPawn(newPawn);
		foreach (Type item2 in from type in typeof(WorkGiver).Assembly.GetTypes()
			where type.IsSubclassOf(typeof(WorkGiver))
			select type)
		{
			WorkGiver item = (WorkGiver)Activator.CreateInstance(item2, pawn);
			workGivers.Add(item);
		}
	}

	public void Notify_WorkPriorityDisabled(WorkType wType)
	{
		if (lastGivenWorkType == wType)
		{
			pawn.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
		}
	}

	protected override Job TryGiveTerminalJob()
	{
		WorkType wt;
		foreach (WorkType item in pawn.MindHuman.workSettings.ActiveWorksByPriority)
		{
			wt = item;
			if (wt.GetDefinition().emergency != emergencyWork)
			{
				continue;
			}
			GenScan.CloseToThingValidator validator = (Thing t) => !t.IsForbidden() && WorkGiverWhoCanStartOn(t, wt) != null;
			Thing thing = GenScan.ClosestReachableThing(pawn.Position, AllPotentialWorkThingsFor(wt), validator);
			if (thing != null)
			{
				lastGivenWorkType = wt;
				WorkGiver workGiver = WorkGiverWhoCanStartOn(thing, wt);
				if (workGiver != null)
				{
					return workGiver.StartingJobOn(thing);
				}
				Debug.LogError(string.Concat("WorkGiver for pawn= ", pawn, " workType= ", wt, " workTarg= ", thing, " was null after it wasn't!"));
			}
		}
		return null;
	}

	private IEnumerable<Thing> AllPotentialWorkThingsFor(WorkType wt)
	{
		foreach (WorkGiver giver in WorkGiversFor(wt))
		{
			foreach (Thing potentialWorkTarget in giver.PotentialWorkTargets)
			{
				yield return potentialWorkTarget;
			}
		}
	}

	private WorkGiver WorkGiverWhoCanStartOn(Thing workTarg, WorkType wt)
	{
		foreach (WorkGiver item in WorkGiversFor(wt))
		{
			if (item.StartingJobOn(workTarg) != null)
			{
				return item;
			}
		}
		return null;
	}

	private IEnumerable<WorkGiver> WorkGiversFor(WorkType work)
	{
		return workGivers.Where((WorkGiver w) => w.wType == work);
	}

	public IEnumerable<Job> ForcingJobsOn(Thing workTarg, WorkType wt)
	{
		if (!wt.GetDefinition().automatic)
		{
			yield break;
		}
		foreach (WorkGiver wGiver in WorkGiversFor(wt))
		{
			Job newJob = wGiver.StartingJobOn(workTarg);
			if (newJob != null)
			{
				yield return newJob;
			}
		}
	}

	public void StartForcedWorkOn(Job forcedJob, Thing workTarg, WorkType wt)
	{
		lastGivenWorkType = wt;
		pawn.MindHuman.workJobQueue.QueueJob(forcedJob);
		pawn.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
	}
}
