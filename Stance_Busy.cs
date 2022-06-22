using System;

public abstract class Stance_Busy : Stance
{
	protected Action finishCallback;

	public TargetPack focusTarg;

	protected int stanceTicksOriginal;

	protected float pieSizeMultiplier = 1f;

	public int stanceTicksLeft;

	public Stance_Busy(int ticks)
		: this(ticks, null, null)
	{
	}

	public Stance_Busy(int ticks, TargetPack focusTarg, Action finishCallback)
	{
		stanceTicksLeft = ticks;
		stanceTicksOriginal = ticks;
		this.finishCallback = finishCallback;
		this.focusTarg = focusTarg;
		if (stanceTicksLeft < 300)
		{
			pieSizeMultiplier = 1f;
		}
		else if (stanceTicksLeft < 450)
		{
			pieSizeMultiplier = 0.75f;
		}
		else
		{
			pieSizeMultiplier = 0.5f;
		}
	}

	public virtual void ResetStanceTicks()
	{
		stanceTicksLeft = stanceTicksOriginal;
	}

	public void SetStateFinishCallback(Action newStateFinishCallback)
	{
		finishCallback = newStateFinishCallback;
	}

	public override void StanceTick()
	{
		if (focusTarg != null && focusTarg.thing != null && focusTarg.thing.destroyed)
		{
			stanceTracker.SetStance(new Stance_Mobile());
			return;
		}
		stanceTicksLeft--;
		if (stanceTicksLeft <= 0)
		{
			stanceTracker.SetStance(new Stance_Mobile());
			if (finishCallback != null)
			{
				finishCallback();
			}
		}
	}

	public override bool StanceBusy()
	{
		return true;
	}
}
