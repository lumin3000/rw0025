using UnityEngine;

public class JobGiver_ExtinguishSelf : ThinkNode_JobGiver
{
	private const float ActivateChance = 0.04f;

	protected override Job TryGiveTerminalJob()
	{
		if (Random.value > 0.04f)
		{
			return null;
		}
		Fire fire = (Fire)pawn.GetAttachment(EntityType.Fire);
		if (fire != null)
		{
			return new Job(JobType.ExtinguishSelf, fire);
		}
		return null;
	}
}
