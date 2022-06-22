public class JobGiver_PrisonerEscape : ThinkNode_JobGiver
{
	protected override Job TryGiveTerminalJob()
	{
		if (Find.Grids.GetRoomAt(pawn.Position) == null && pawn.prisoner.Secure)
		{
			bool succeeded;
			IntVec3 targetLoc = ExitUtility.ClosestExitSpotTo(pawn.Position, out succeeded);
			if (succeeded)
			{
				UI_Messages.Message("Prisoner " + pawn.characterName + " is escaping.", UIMessageSound.SeriousAlert);
				Job job = new Job(JobType.Goto, new TargetPack(targetLoc));
				job.exitMapOnArrival = true;
				return job;
			}
		}
		return null;
	}
}
