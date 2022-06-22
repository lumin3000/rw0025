public abstract class ThinkNode_JobGiver : ThinkNode
{
	protected abstract Job TryGiveTerminalJob();

	public override JobPackage TryIssueJobPackage()
	{
		Job job = TryGiveTerminalJob();
		if (job == null)
		{
			return null;
		}
		return new JobPackage(job, this);
	}
}
