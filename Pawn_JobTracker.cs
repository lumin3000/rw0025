using UnityEngine;

public class Pawn_JobTracker : Saveable
{
	protected Pawn pawn;

	private Job CurJobInt;

	private JobDriver CurJobDriverInt;

	public Job CurJob
	{
		get
		{
			return CurJobInt;
		}
		protected set
		{
			CurJobInt = value;
		}
	}

	public JobDriver CurJobDriver
	{
		get
		{
			return CurJobDriverInt;
		}
		protected set
		{
			CurJobDriverInt = value;
		}
	}

	public Pawn_JobTracker(Pawn newPawn)
	{
		pawn = newPawn;
	}

	public virtual void ExposeData()
	{
		Scribe.LookSaveable(ref CurJobInt, "CurJob");
		Scribe.LookSaveable(ref CurJobDriverInt, "CurJobDriver", pawn);
	}

	public virtual void JobTrackerTick()
	{
		if (CurJobDriver != null)
		{
			if (Find.TickManager.tickCount > CurJob.expiryTime)
			{
				EndCurrentJob(JobCondition.Succeeded);
			}
			else
			{
				CurJobDriver.DriverTick();
			}
		}
	}

	public void StartJob(Job newJob)
	{
		if (CurJob != null)
		{
			Debug.LogWarning(string.Concat(pawn, " starting job ", newJob, " while already having job ", CurJob));
			EndCurrentJob(JobCondition.ForcedInterrupt);
		}
		if (newJob == null)
		{
			Debug.LogWarning(string.Concat(pawn, " tried to start doing a null job."));
			return;
		}
		CurJob = newJob;
		CurJobDriver = CurJob.GetDriver(pawn);
		CurJobDriver.DriverStart();
	}

	public void EndCurrentJob(JobCondition condition)
	{
		if (CurJob == null)
		{
			Debug.LogWarning(string.Concat(pawn, " did EndCurrentJob without having a job. Not a huge deal, but sketchy."));
			return;
		}
		Job curJob = CurJob;
		CurJobDriver.DriverCleanup(condition);
		CurJobDriver = null;
		CurJob = null;
		Find.ReservationManager.UnReserveAllForPawn(pawn);
		if (pawn.carryHands.carriedThing != null)
		{
			pawn.carryHands.DropCarriedThing();
		}
		if (condition == JobCondition.PatherFailed)
		{
			pawn.jobs.StartJob(new Job(JobType.Wait, 30));
		}
		else
		{
			pawn.mind.Notify_JobEnded(curJob, condition);
		}
	}

	public void Notify_Carried()
	{
		EndCurrentJob(JobCondition.Incompletable);
	}

	public void Notify_IncapacitatedOrKilled()
	{
		if (CurJob != null)
		{
			EndCurrentJob(JobCondition.Incompletable);
			CurJob = null;
			CurJobDriver = null;
		}
	}

	public void Notify_TuckedIntoBed(Building_Bed DropBed)
	{
		pawn.Position = DropBed.Position;
		pawn.Notify_Teleported();
		StartJob(new Job(JobType.Sleep, new TargetPack(DropBed)));
	}

	public void Notify_DamageTaken(DamageInfo d)
	{
		if (CurJobDriver != null)
		{
			CurJobDriver.Notify_DamageTaken(d);
		}
	}
}
