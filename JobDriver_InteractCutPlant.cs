using UnityEngine;

public class JobDriver_InteractCutPlant : JobDriver_Interact
{
	public JobDriver_InteractCutPlant(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Cutting " + base.TargetThingA.Label + ".", JobReportOverlays.grower);
	}

	public override void DriverCleanup(JobCondition condition)
	{
		base.DriverCleanup(condition);
		if (condition == JobCondition.Succeeded)
		{
			base.CurJob.targetA.thing.Destroy();
			AudioClip clip = GenSound.RandomClipInFolder("Interaction/Farming", NoRepeat: true);
			GenSound.PlaySoundAt(pawn.Position, clip);
		}
	}
}
