public class MindState : Saveable
{
	public Pawn pawn;

	public MindBrokenState brokenState;

	public JobTag lastJobTag;

	public ThinkNode lastJobGiver;

	public AIDuty duty;

	public IntVec3 dutyLocation;

	public Thing enemyTarget;

	public Pawn closeThreat;

	public int lastCloseThreatHarmTime;

	public IntVec3 travelDestination;

	public bool IsIdle
	{
		get
		{
			if (pawn.Incapacitated)
			{
				return false;
			}
			return lastJobTag == JobTag.Idle;
		}
	}

	public MindState(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		if (enemyTarget != null && enemyTarget.destroyed)
		{
			enemyTarget = null;
		}
		Scribe.LookField(ref brokenState, "BrokenState", MindBrokenState.Unbroken);
		Scribe.LookField(ref lastJobTag, "LastJobTag", JobTag.NoTag);
		Scribe.LookField(ref duty, "Duty");
		Scribe.LookField(ref dutyLocation, "DutyLocation");
		Scribe.LookThingRef(ref enemyTarget, "EnemyTarget", this);
		Scribe.LookField(ref travelDestination, "TravelDestination");
	}

	public void Notify_WorkPriorityDisabled(WorkType wType)
	{
		(lastJobGiver as JobGiver_WorkRoot)?.Notify_WorkPriorityDisabled(wType);
	}

	public void Notify_IncappedOrKilled()
	{
		enemyTarget = null;
		closeThreat = null;
		lastCloseThreatHarmTime = 0;
		lastJobGiver = null;
		lastJobTag = JobTag.NoTag;
		brokenState = MindBrokenState.Unbroken;
	}
}
