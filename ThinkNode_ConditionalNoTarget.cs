public class ThinkNode_ConditionalNoTarget : ThinkNode_Priority
{
	public override JobPackage TryIssueJobPackage()
	{
		if (pawn.MindState.enemyTarget != null)
		{
			return null;
		}
		return base.TryIssueJobPackage();
	}
}
