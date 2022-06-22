using System;

public class Toil
{
	public JobDriverToil owningDriver;

	public Action initAction;

	public ToilCompleteMode defaultCompleteMode;

	public Action tickAction;

	public Func<bool> tickFailCondition;

	public Action<DamageInfo> notify_DamageTakenAction;

	public int duration;

	public Toil()
	{
	}

	public Toil(Action InitAction, ToilCompleteMode CompleteMode)
	{
		initAction = InitAction;
		defaultCompleteMode = CompleteMode;
	}
}
