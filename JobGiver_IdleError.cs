using UnityEngine;

public class JobGiver_IdleError : ThinkNode_JobGiver
{
	private const int WaitTime = 100;

	protected override Job TryGiveTerminalJob()
	{
		Debug.LogWarning(string.Concat(pawn, " issued IdleError wait job. The behavior tree should never get here."));
		Job job = new Job(JobType.Wait);
		job.TimeLimit = 100;
		return job;
	}
}
