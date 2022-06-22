using System;

public class JobDriver_InteractClean : JobDriver_Interact
{
	public JobDriver_InteractClean(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Cleaning " + base.TargetThingA.Label + ".", JobReportOverlays.miner);
	}

	protected override Func<bool> GetInteractFailCondition()
	{
		return delegate
		{
			if (!ToilTools.CanInteractStandard(pawn, base.TargetThingA))
			{
				return true;
			}
			return (!Find.HomeZoneGrid.ShouldClean(base.TargetThingA.Position)) ? true : false;
		};
	}
}
