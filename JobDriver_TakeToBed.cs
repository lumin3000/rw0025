using System.Collections.Generic;

public class JobDriver_TakeToBed : JobDriverToil
{
	protected Pawn Takee => (Pawn)base.CurJob.targetA.thing;

	protected Building_Bed DropBed => (Building_Bed)base.CurJob.targetB.thing;

	public JobDriver_TakeToBed(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		if (base.CurJob.jType == JobType.Capture)
		{
			return new JobReport("Capturing " + Takee.characterName + ".", JobReportOverlays.warden);
		}
		if (base.CurJob.jType == JobType.Rescue)
		{
			return new JobReport("Rescuing " + Takee.characterName + ".", JobReportOverlays.doctor);
		}
		if (base.CurJob.jType == JobType.Arrest)
		{
			return new JobReport("Arresting " + Takee.characterName + ".", JobReportOverlays.warden);
		}
		if (base.CurJob.jType == JobType.EscortPrisonerToBed)
		{
			return new JobReport("Escorting prisoner " + Takee.characterName + ".", JobReportOverlays.warden);
		}
		if (base.CurJob.jType == JobType.TakeWoundedPrisonerToBed)
		{
			return new JobReport("Taking wounded prisoner " + Takee.characterName + " to bed.", JobReportOverlays.warden);
		}
		return JobReport.Error;
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil
		{
			initAction = delegate
			{
				if (Takee.ownership.ownedBed != DropBed)
				{
					Takee.ownership.ClaimBed(DropBed);
				}
				Find.ReservationManager.ReserveFor(pawn, Takee, ReservationType.Total);
				pawn.pather.StartPathTowards(new TargetPack(Takee));
			},
			tickFailCondition = delegate
			{
				if (DropBed.owner != Takee)
				{
					return true;
				}
				return (base.CurJob.jType == JobType.Arrest && !Takee.Team.CanBeArrested()) || !ToilTools.CanInteractStandard(pawn, DropBed) || !ToilTools.CanInteractStandard(pawn, Takee);
			},
			defaultCompleteMode = ToilCompleteMode.PatherArrival
		};
		yield return new Toil
		{
			initAction = delegate
			{
				Find.ReservationManager.UnReserve(Takee, ReservationType.Total);
				pawn.carryHands.StartCarry(Takee);
				TeamType teamType = Takee.Team;
				if (base.CurJob.jType == JobType.Arrest || base.CurJob.jType == JobType.Capture)
				{
					teamType = TeamType.Prisoner;
				}
				if (Takee.Team != teamType)
				{
					Takee.ChangePawnTeamTo(teamType);
					if (Takee.inventory.Has(EntityType.DoorKey))
					{
						Takee.inventory.DestroyInventory(EntityType.DoorKey);
					}
				}
				pawn.pather.StartPathTowards(DropBed);
			},
			tickFailCondition = () => Takee.destroyed ? true : false,
			defaultCompleteMode = ToilCompleteMode.PatherArrival
		};
		yield return new Toil
		{
			initAction = delegate
			{
				pawn.carryHands.DropCarriedThing();
				if (Takee.Incapacitated || Takee.Team != TeamType.Colonist)
				{
					Takee.Position = DropBed.Position;
					Takee.Notify_Teleported();
				}
				if (Takee.Incapacitated && DropBed.owner == Takee && !DropBed.destroyed)
				{
					Takee.jobs.Notify_TuckedIntoBed(DropBed);
				}
				if (Takee.Team == TeamType.Prisoner)
				{
					Find.Tutor.Signal(TutorSignal.PrisonerInBed, Takee);
				}
			},
			defaultCompleteMode = ToilCompleteMode.Immediate
		};
	}

	public override void DriverCleanup(JobCondition condition)
	{
		base.DriverCleanup(condition);
		if (Find.ReservationManager.ReserverOf(DropBed, ReservationType.UseDevice) == Takee)
		{
			Find.ReservationManager.UnReserve(DropBed, ReservationType.UseDevice);
		}
	}
}
