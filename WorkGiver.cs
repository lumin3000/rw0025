using System.Collections;

public abstract class WorkGiver
{
	protected Pawn pawn;

	public WorkType wType;

	public abstract IEnumerable PotentialWorkTargets { get; }

	public WorkGiver(Pawn newPawn)
	{
		pawn = newPawn;
	}

	public abstract Job StartingJobOn(Thing t);
}
