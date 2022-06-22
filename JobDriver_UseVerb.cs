using System.Collections.Generic;

public class JobDriver_UseVerb : JobDriverToil
{
	public JobDriver_UseVerb(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		string text = ((!base.TargetA.HasThing) ? "ground" : base.TargetThingA.Label);
		return new JobReport("Using " + base.CurJob.verbToUse.VerbDef.label + " on " + text + ".", null);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil
		{
			initAction = delegate
			{
				GenAI.AIDestination aIDestination = CastPositionFinder.FindCastingPosition(new CastingPositionRequest
				{
					maxRangeFromTarget = base.CurJob.verbToUse.VerbDef.range,
					moverPawn = pawn,
					targetThing = base.TargetThingA,
					wantCoverFromTarget = false
				});
				if (!aIDestination.found)
				{
					EndJobWith(JobCondition.Incompletable);
				}
				else
				{
					pawn.pather.StartPathTowards(aIDestination.pos);
				}
			},
			tickFailCondition = () => base.TargetThingA.destroyed ? true : false,
			defaultCompleteMode = ToilCompleteMode.PatherArrival
		};
		yield return new Toil
		{
			initAction = delegate
			{
				base.CurJob.verbToUse.TryStartCastOn(base.TargetThingA);
			},
			tickFailCondition = () => base.TargetThingA.destroyed ? true : false,
			defaultCompleteMode = ToilCompleteMode.PatherArrival
		};
	}
}
