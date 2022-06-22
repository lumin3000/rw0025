public class JobGiver_Orders : ThinkNode_JobGiver
{
	private Job queuedJob;

	private Job DefaultJob => new Job(JobType.Wait, new TargetPack(pawn.Position));

	protected override Job TryGiveTerminalJob()
	{
		if (queuedJob != null)
		{
			Job result = queuedJob;
			queuedJob = null;
			return result;
		}
		if (pawn.MindHuman.drafted)
		{
			return DefaultJob;
		}
		return null;
	}

	public void QueueJob(Job newJob)
	{
		queuedJob = newJob;
	}
}
