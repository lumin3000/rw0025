using System.Collections.Generic;

public class JobDriver_BeatFire : JobDriverToil
{
	protected Fire TargetFire => (Fire)base.CurJob.targetA.thing;

	public JobDriver_BeatFire(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Beating fire out.", null);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil
		{
			initAction = delegate
			{
				pawn.TryReserve(TargetFire, ReservationType.Total);
			},
			tickAction = delegate
			{
				if (pawn.natives.CanTouch(TargetFire))
				{
					pawn.natives.TryBeatFire(TargetFire);
				}
				else if (!pawn.pather.moving)
				{
					pawn.pather.StartPathTowards(TargetFire);
				}
			},
			tickFailCondition = ToilTools.StandardTickFail(pawn, TargetFire),
			defaultCompleteMode = ToilCompleteMode.Never
		};
	}
}
