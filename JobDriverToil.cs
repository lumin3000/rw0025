using System.Collections.Generic;
using System.Linq;

public abstract class JobDriverToil : JobDriver
{
	private List<Toil> toilList = new List<Toil>();

	private int curToilIndex = -1;

	private ToilCompleteMode curToilCompleteMode;

	public int ticksInToil;

	protected Toil CurToil
	{
		get
		{
			if (curToilIndex < 0)
			{
				return null;
			}
			if (curToilIndex >= toilList.Count)
			{
				return null;
			}
			return toilList[curToilIndex];
		}
	}

	public JobDriverToil(Pawn pawn)
		: base(pawn)
	{
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref curToilIndex, "CurToilIndex", forceSave: true);
		Scribe.LookField(ref ticksInToil, "TicksInToil", 0);
		Scribe.LookField(ref curToilCompleteMode, "CurToilCompleteMode");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			MakeAndFinalizeToilSeq();
		}
	}

	public override void DriverStart()
	{
		MakeAndFinalizeToilSeq();
		BeginNextToil();
	}

	private void MakeAndFinalizeToilSeq()
	{
		toilList = NewToilList().ToList();
		foreach (Toil toil in toilList)
		{
			toil.owningDriver = this;
		}
	}

	protected abstract IEnumerable<Toil> NewToilList();

	public override void DriverTick()
	{
		if (CurToil.tickFailCondition != null && CurToil.tickFailCondition())
		{
			EndJobWith(JobCondition.Incompletable);
			return;
		}
		if (curToilCompleteMode == ToilCompleteMode.Delay)
		{
			ticksInToil++;
			if (ticksInToil >= CurToil.duration)
			{
				BeginNextToil();
				return;
			}
		}
		if (CurToil.tickAction != null)
		{
			CurToil.tickAction();
		}
	}

	public override void Notify_PatherArrived()
	{
		if (curToilCompleteMode == ToilCompleteMode.PatherArrival)
		{
			BeginNextToil();
		}
	}

	public override void Notify_DamageTaken(DamageInfo d)
	{
		if (CurToil.notify_DamageTakenAction != null)
		{
			CurToil.notify_DamageTakenAction(d);
		}
		base.Notify_DamageTaken(d);
	}

	public void BeginNextToil()
	{
		ticksInToil = 0;
		curToilIndex++;
		if (CurToil != null)
		{
			curToilCompleteMode = CurToil.defaultCompleteMode;
			if (CurToil.initAction != null)
			{
				CurToil.initAction();
			}
			if (curToilCompleteMode == ToilCompleteMode.Immediate)
			{
				BeginNextToil();
			}
		}
		else
		{
			EndJobWith(JobCondition.Succeeded);
		}
	}

	public void SetNextToil(Toil to)
	{
		curToilIndex = toilList.IndexOf(to) - 1;
	}

	public void SetCompleteMode(ToilCompleteMode compMode)
	{
		curToilCompleteMode = compMode;
	}
}
