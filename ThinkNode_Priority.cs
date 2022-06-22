public class ThinkNode_Priority : ThinkNode
{
	public override JobPackage TryIssueJobPackage()
	{
		foreach (ThinkNode subNode in subNodes)
		{
			JobPackage jobPackage = subNode.TryIssueJobPackage();
			if (jobPackage != null)
			{
				return jobPackage;
			}
		}
		return null;
	}
}
