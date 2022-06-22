using System;
using UnityEngine;

public class JobDriver_InteractRepair : JobDriver_Interact
{
	private const int HealthPerRepair = 1;

	private const float TicksBetweenRepairs = 7f;

	protected float ticksToNextRepair;

	protected override Type EffectMakerType => base.TargetThingA.def.repairEffects;

	public JobDriver_InteractRepair(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Repairing " + base.TargetThingA.Label + ".", JobReportOverlays.repairer);
	}

	public override void Notify_PatherFailed()
	{
		Debug.LogWarning(string.Concat("Can't repair ", base.TargetThingA, " due to lacking a path. Cheating and instant-repairing it..."));
		base.TargetThingA.health = base.TargetThingA.def.maxHealth;
		base.Notify_PatherFailed();
	}

	protected override JobCondition InteractionTick()
	{
		DriveEffects();
		pawn.skills.Learn(SkillType.Construction, 0.7f);
		float num = 0.5f + 0.15f * (float)pawn.skills.LevelOf(SkillType.Construction);
		num *= pawn.healthTracker.CurEffectivenessPercent;
		ticksToNextRepair -= num;
		if (ticksToNextRepair <= 0f)
		{
			ticksToNextRepair = 7f;
			base.TargetThingA.health++;
			if (base.TargetThingA.health > base.TargetThingA.def.maxHealth)
			{
				base.TargetThingA.health = base.TargetThingA.def.maxHealth;
			}
			if (base.TargetThingA.health == base.TargetThingA.def.maxHealth)
			{
				return JobCondition.Succeeded;
			}
		}
		return JobCondition.Ongoing;
	}
}
