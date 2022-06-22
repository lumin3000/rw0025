public class JobGiver_AIBuffAllies : ThinkNode_JobGiver
{
	protected const int TicksBetweenActionsMin = 700;

	protected const int TicksBetweenActionsMax = 900;

	protected const int CastingLocFailedWaitTime = 25;

	protected const int ApproachDistance = 3;

	protected Pawn AllyToBuff;

	protected int TicksUntilNextAction;

	protected Verb BuffToUse => null;

	protected override Job TryGiveTerminalJob()
	{
		return null;
	}
}
