public abstract class JobGiver_ExitMap : ThinkNode_JobGiver
{
	protected MoveSpeed moveSpeed = MoveSpeed.Jog;

	protected override Job TryGiveTerminalJob()
	{
		bool succeeded;
		IntVec3 targetLoc = GoodExitDest(out succeeded);
		if (succeeded)
		{
			Job job = new Job(JobType.Goto, new TargetPack(targetLoc));
			job.exitMapOnArrival = true;
			job.moveSpeed = moveSpeed;
			return job;
		}
		return null;
	}

	protected abstract IntVec3 GoodExitDest(out bool succeeded);
}
