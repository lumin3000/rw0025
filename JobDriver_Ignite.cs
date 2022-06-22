using System.Collections.Generic;

public class JobDriver_Ignite : JobDriverToil
{
	public JobDriver_Ignite(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Igniting " + base.TargetThingA.Label + ".", null);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil
		{
			initAction = delegate
			{
			},
			tickAction = delegate
			{
				if (pawn.natives.CanTouch(base.TargetThingA))
				{
					pawn.natives.TryIgnite(base.TargetThingA);
				}
				else if (!pawn.pather.moving)
				{
					pawn.pather.StartPathTowards(base.TargetThingA);
				}
			},
			tickFailCondition = () => (base.TargetThingA.destroyed || base.TargetThingA.IsBurningImmobile()) ? true : false,
			defaultCompleteMode = ToilCompleteMode.Never
		};
	}
}
