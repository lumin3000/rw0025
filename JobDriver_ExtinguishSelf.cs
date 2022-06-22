using System.Collections.Generic;

public class JobDriver_ExtinguishSelf : JobDriverToil
{
	protected const int NumSpeechesToSay = 5;

	protected Fire TargetFire => (Fire)base.CurJob.targetA.thing;

	public JobDriver_ExtinguishSelf(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		string text = "Extinguishing fire.";
		if (TargetFire.parent != null)
		{
			text = "Extinguishing fire on " + TargetFire.parent.Label + ".";
		}
		return new JobReport(text, null);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil
		{
			initAction = delegate
			{
			},
			defaultCompleteMode = ToilCompleteMode.Delay,
			duration = 150
		};
		yield return new Toil
		{
			initAction = delegate
			{
				TargetFire.Destroy();
			},
			defaultCompleteMode = ToilCompleteMode.Immediate
		};
	}
}
