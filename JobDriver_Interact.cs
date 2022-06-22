using System;
using System.Collections.Generic;
using UnityEngine;

public class JobDriver_Interact : JobDriverToil
{
	private const int TicksBetweenBreaks = 400;

	private EffectMaker interactionEffectMaker;

	private int ticksUntilBreak = 400;

	private MoteAttached interactMote;

	protected virtual Type EffectMakerType => base.CurJob.Def.interactEffectMakerType;

	public JobDriver_Interact(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		if (base.CurJob.Def.jobReportSpecial != null)
		{
			return base.CurJob.Def.jobReportSpecial;
		}
		return new JobReport("Using " + base.TargetThingA.Label + ".", null);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil(delegate
		{
			IntVec3 intVec = default(IntVec3);
			if (base.CurJob.Def.interactLocation == InteractionLocationType.OnPosition)
			{
				intVec = base.TargetThingA.Position;
			}
			else if (base.CurJob.Def.interactLocation == InteractionLocationType.InteractionSquare)
			{
				intVec = ((Building)base.TargetThingA).InteractionSquare;
			}
			else if (base.CurJob.Def.interactLocation == InteractionLocationType.AnyAdjacent)
			{
				intVec = GenPath.SpotToStandAdjacentToFor(pawn, base.TargetThingA, out var succeeded);
				if (!succeeded)
				{
					Debug.LogWarning(string.Concat(pawn, " failed to find spot adjacent to ", base.TargetThingA, " at ", base.TargetThingA.Position, " when interacting. This should have been caught earlier."));
					EndJobWith(JobCondition.PatherFailed);
					return;
				}
			}
			if (!pawn.TryReserve(base.TargetThingA, base.CurJob.Def.reservationType))
			{
				Debug.LogWarning(string.Concat(pawn, " interacting: JobDriver could not reserve ", base.TargetThingA, " for interaction type ", base.CurJob.Def.reservationType, ". Ending job."));
				EndJobWith(JobCondition.Incompletable);
			}
			else
			{
				Find.PawnDestinationManager.ReserveDestinationFor(pawn, intVec);
				pawn.pather.StartPathTowards(intVec);
			}
		}, ToilCompleteMode.PatherArrival)
		{
			tickFailCondition = GetInteractFailCondition()
		};
		yield return new Toil(delegate
		{
		}, ToilCompleteMode.Never)
		{
			tickAction = delegate
			{
				JobCondition jobCondition = InteractionTick();
				if (jobCondition != 0)
				{
					EndJobWith(jobCondition);
				}
				else if (base.CurJob.Def.interactTakeBreaks)
				{
					ticksUntilBreak--;
					if (ticksUntilBreak == 0)
					{
						EndJobWith(JobCondition.Succeeded);
					}
				}
			},
			notify_DamageTakenAction = delegate(DamageInfo d)
			{
				if (base.CurJob.jType == JobType.Sleep && d.type.HarmsHealth() && !pawn.Incapacitated)
				{
					EndJobWith(JobCondition.Incompletable);
				}
			},
			tickFailCondition = GetInteractFailCondition()
		};
	}

	protected virtual JobCondition InteractionTick()
	{
		DriveEffects();
		return ((Interactive)base.TargetThingA).InteractedWith(base.CurJob.Def.reservationType, pawn);
	}

	public override void DriverCleanup(JobCondition condition)
	{
		if (interactMote != null)
		{
			interactMote.Destroy();
		}
		base.DriverCleanup(condition);
	}

	protected void DriveEffects()
	{
		if (interactMote == null && base.CurJob.Def.interactMoteName != string.Empty)
		{
			interactMote = MoteMaker.MakeInteractionOverlay(base.CurJob.Def.interactMoteName, pawn, base.TargetThingA);
		}
		if (EffectMakerType != null)
		{
			if (interactionEffectMaker == null)
			{
				interactionEffectMaker = (EffectMaker)Activator.CreateInstance(EffectMakerType);
			}
			interactionEffectMaker.EffectTick(pawn, base.CurJob.targetA);
		}
	}

	protected virtual Func<bool> GetInteractFailCondition()
	{
		return () => !ToilTools.CanInteractStandard(pawn, base.TargetThingA) || (base.CurJob.Def.interactFailCondition != null && base.CurJob.Def.interactFailCondition(base.TargetThingA));
	}
}
