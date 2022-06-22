using UnityEngine;

public class JobDriver_InteractHarvest : JobDriver_Interact
{
	public JobDriver_InteractHarvest(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Harvesting " + base.TargetThingA.Label + ".", JobReportOverlays.grower);
	}

	public override void DriverCleanup(JobCondition condition)
	{
		base.DriverCleanup(condition);
		if (condition == JobCondition.Succeeded)
		{
			if (pawn.skills != null)
			{
				pawn.skills.Learn(SkillType.Growing, 50f);
			}
			AudioClip clip = GenSound.RandomClipInFolder("Interaction/Farming", NoRepeat: true);
			GenSound.PlaySoundAt(pawn.Position, clip);
		}
	}
}
