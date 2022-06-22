public class JobGiver_GetFood : ThinkNode_JobGiver
{
	protected override Job TryGiveTerminalJob()
	{
		Thing thing = FoodUtility.FindFoodSourceFor(pawn);
		if (thing == null)
		{
			return null;
		}
		return new Job(JobType.EatFood, thing);
	}
}
