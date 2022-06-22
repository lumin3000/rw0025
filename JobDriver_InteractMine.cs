using System;

public class JobDriver_InteractMine : JobDriver_Interact
{
	public JobDriver_InteractMine(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Mining " + base.TargetThingA.Label + ".", JobReportOverlays.miner);
	}

	protected override Func<bool> GetInteractFailCondition()
	{
		return delegate
		{
			if (!ToilTools.CanInteractStandard(pawn, base.TargetThingA))
			{
				return true;
			}
			return (Find.DesignationManager.DesignationAt(base.TargetThingA.Position, DesignationType.Mine) == null) ? true : false;
		};
	}
}
