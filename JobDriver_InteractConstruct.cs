using System;

public class JobDriver_InteractConstruct : JobDriver_Interact
{
	protected override Type EffectMakerType => base.TargetThingA.def.constructionEffects;

	public JobDriver_InteractConstruct(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Building " + base.TargetThingA.Label + ".", JobReportOverlays.constructor);
	}
}
