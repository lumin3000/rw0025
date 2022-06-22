public class ThinkNode_ConditionalBroken : ThinkNode_Priority
{
	private MindBrokenState brokenState;

	public ThinkNode_ConditionalBroken(MindBrokenState brokenState)
	{
		this.brokenState = brokenState;
	}

	public override JobPackage TryIssueJobPackage()
	{
		if (pawn.MindState.brokenState != brokenState)
		{
			return null;
		}
		return base.TryIssueJobPackage();
	}
}
