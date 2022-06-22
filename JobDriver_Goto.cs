public class JobDriver_Goto : JobDriver
{
	public JobDriver_Goto(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Walking.", null);
	}

	public override void DriverStart()
	{
		pawn.pather.StartPathTowards(base.CurJob.targetA);
	}

	public override void Notify_PatherArrived()
	{
		if (base.CurJob.exitMapOnArrival)
		{
			pawn.Destroy();
		}
		else
		{
			EndJobWith(JobCondition.Succeeded);
		}
	}
}
