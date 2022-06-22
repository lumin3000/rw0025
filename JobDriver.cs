using UnityEngine;

public abstract class JobDriver : Saveable
{
	public Pawn pawn;

	protected JobType JType => pawn.jobs.CurJob.jType;

	protected Job CurJob => pawn.jobs.CurJob;

	protected TargetPack TargetA => pawn.jobs.CurJob.targetA;

	protected Thing TargetThingA
	{
		get
		{
			return pawn.jobs.CurJob.targetA.thing;
		}
		set
		{
			pawn.jobs.CurJob.targetA = value;
		}
	}

	protected Thing TargetThingB
	{
		get
		{
			return pawn.jobs.CurJob.targetB.thing;
		}
		set
		{
			pawn.jobs.CurJob.targetB = value;
		}
	}

	protected IntVec3 TargetLocA => pawn.jobs.CurJob.targetA.Loc;

	public JobDriver(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void EndJobWith(JobCondition condition)
	{
		if (condition == JobCondition.Ongoing)
		{
			Debug.LogWarning("You can't end a job with Ongoing as the condition.");
		}
		pawn.jobs.EndCurrentJob(condition);
	}

	public virtual void ExposeData()
	{
	}

	public virtual void DriverStart()
	{
	}

	public virtual void DriverTick()
	{
	}

	public virtual void DriverCleanup(JobCondition condition)
	{
	}

	public abstract JobReport GetReport();

	public virtual void Notify_PatherArrived()
	{
	}

	public virtual void Notify_PatherFailed()
	{
		EndJobWith(JobCondition.PatherFailed);
	}

	public virtual void Notify_DamageTaken(DamageInfo d)
	{
		if (CurJob.Def.interruptOnHarmfulDamage && d.type.HarmsHealth())
		{
			EndJobWith(JobCondition.Incompletable);
		}
	}
}
