using System;

public class ThinkNode_ConditionalFunc : ThinkNode_Priority
{
	public Func<bool> condition;

	public override JobPackage TryIssueJobPackage()
	{
		if (!condition())
		{
			return null;
		}
		return base.TryIssueJobPackage();
	}
}
