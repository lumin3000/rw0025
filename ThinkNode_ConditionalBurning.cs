public class ThinkNode_ConditionalBurning : ThinkNode_Priority
{
	public override JobPackage TryIssueJobPackage()
	{
		if (!pawn.HasAttachment(EntityType.Fire))
		{
			return null;
		}
		return base.TryIssueJobPackage();
	}
}
