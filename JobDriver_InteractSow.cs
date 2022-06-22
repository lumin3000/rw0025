using System.Collections.Generic;
using UnityEngine;

public class JobDriver_InteractSow : JobDriver_Interact
{
	public JobDriver_InteractSow(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Sowing " + base.CurJob.plantDefToSow.label + ".", JobReportOverlays.grower);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil(delegate
		{
			if (!pawn.CanReach(base.TargetLocA, adjacentIsOK: true))
			{
				Debug.LogWarning(string.Concat(pawn, " failed to find spot adjacent to ", base.TargetLocA, " when sowing. This should have been caught earlier."));
				EndJobWith(JobCondition.PatherFailed);
			}
			else
			{
				Find.ReservationManager.ReserveFor(pawn, base.TargetLocA, ReservationType.Sowing);
				pawn.pather.StartPathTowards(base.TargetLocA, autoReserve: false, adjacentIsOK: true);
			}
		}, ToilCompleteMode.PatherArrival)
		{
			tickFailCondition = null
		};
		yield return new Toil
		{
			initAction = delegate
			{
				base.TargetThingA = ThingMaker.Spawn(base.CurJob.plantDefToSow, base.TargetLocA);
			},
			defaultCompleteMode = ToilCompleteMode.Never,
			tickAction = delegate
			{
				if (base.TargetThingA == null)
				{
					Debug.LogError(string.Concat(pawn, " sowing and TargetThingA == null"));
					EndJobWith(JobCondition.Incompletable);
				}
				else
				{
					DriveEffects();
					JobCondition jobCondition = ((Interactive)base.TargetThingA).InteractedWith(ReservationType.Sowing, pawn);
					if (jobCondition != 0)
					{
						EndJobWith(jobCondition);
					}
				}
			},
			tickFailCondition = () => base.TargetThingA.destroyed
		};
	}

	public override void DriverCleanup(JobCondition condition)
	{
		base.DriverCleanup(condition);
		if (condition == JobCondition.Succeeded)
		{
			pawn.skills.Learn(SkillType.Growing, 25f);
			GenSound.PlaySoundAt(pawn.Position, GenSound.RandomClipInFolder("Interaction/Farming", NoRepeat: true));
		}
		else if (base.TargetThingA != null)
		{
			base.TargetThingA.Destroy();
		}
	}
}
