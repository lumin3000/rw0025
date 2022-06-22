public class ThinkNode_Random : ThinkNode
{
	public override JobPackage TryIssueJobPackage()
	{
		foreach (ThinkNode item in subNodes.InRandomOrder())
		{
			JobPackage jobPackage = item.TryIssueJobPackage();
			if (jobPackage != null)
			{
				return jobPackage;
			}
		}
		return null;
	}
}
