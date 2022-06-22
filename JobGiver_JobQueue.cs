using System.Collections.Generic;
using UnityEngine;

public class JobGiver_JobQueue : ThinkNode_JobGiver
{
	private Queue<Job> jobQueue = new Queue<Job>();

	protected override Job TryGiveTerminalJob()
	{
		if (jobQueue.Count == 0)
		{
			return null;
		}
		return jobQueue.Dequeue();
	}

	public void QueueJob(Job newJob)
	{
		if (newJob == null)
		{
			Debug.LogError("Cannot queue null job");
		}
		else
		{
			jobQueue.Enqueue(newJob);
		}
	}
}
