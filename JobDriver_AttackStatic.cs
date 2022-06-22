public class JobDriver_AttackStatic : JobDriver
{
	private bool startedIncapacitated;

	public JobDriver_AttackStatic(Pawn pawn)
		: base(pawn)
	{
	}

	public override void DriverStart()
	{
		Pawn pawn = base.TargetThingA as Pawn;
		if (pawn != null)
		{
			startedIncapacitated = pawn.Incapacitated;
		}
		base.pawn.pather.StopDead();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref startedIncapacitated, "StartedIncapacitated");
	}

	public override JobReport GetReport()
	{
		string text = ((!base.TargetA.HasThing) ? "area" : base.TargetA.thing.Label);
		return new JobReport("Attacking " + text + ".", JobReportOverlays.warden);
	}

	public override void DriverTick()
	{
		if (base.TargetA.HasThing)
		{
			Pawn pawn = base.TargetA.thing as Pawn;
			if (base.TargetA.thing.destroyed || (pawn != null && !startedIncapacitated && pawn.Incapacitated))
			{
				EndJobWith(JobCondition.Succeeded);
				return;
			}
		}
		base.pawn.equipment.TryStartAttack(base.TargetA);
	}
}
