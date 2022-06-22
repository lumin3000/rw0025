public class ThinkNode_Tagger : ThinkNode_Priority
{
	private JobTag tagToGive;

	public ThinkNode_Tagger(JobTag tagToGive)
	{
		this.tagToGive = tagToGive;
	}

	public override JobPackage TryIssueJobPackage()
	{
		JobPackage jobPackage = base.TryIssueJobPackage();
		if (jobPackage != null)
		{
			pawn.MindState.lastJobTag = tagToGive;
		}
		return jobPackage;
	}
}
