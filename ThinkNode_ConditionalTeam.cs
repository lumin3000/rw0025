public class ThinkNode_ConditionalTeam : ThinkNode_Priority
{
	private TeamType team;

	public ThinkNode_ConditionalTeam(TeamType team)
	{
		this.team = team;
	}

	public override JobPackage TryIssueJobPackage()
	{
		if (pawn.Team != team)
		{
			return null;
		}
		return base.TryIssueJobPackage();
	}
}
