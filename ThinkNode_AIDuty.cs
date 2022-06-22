public class ThinkNode_AIDuty : ThinkNode
{
	public ThinkNode nodeAssault;

	public ThinkNode nodeDefend;

	public ThinkNode nodeStage;

	public ThinkNode nodeExit;

	public override JobPackage TryIssueJobPackage()
	{
		if (pawn.GetKing() == null)
		{
			return nodeAssault.TryIssueJobPackage();
		}
		return pawn.MindState.duty switch
		{
			AIDuty.Stage => nodeStage.TryIssueJobPackage(), 
			AIDuty.Assault => nodeAssault.TryIssueJobPackage(), 
			AIDuty.Defend => nodeDefend.TryIssueJobPackage(), 
			AIDuty.Exit => nodeExit.TryIssueJobPackage(), 
			_ => null, 
		};
	}
}
