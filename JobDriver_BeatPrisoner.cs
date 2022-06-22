using System.Collections.Generic;

public class JobDriver_BeatPrisoner : JobDriver_AttackMelee
{
	public JobDriver_BeatPrisoner(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		string text = "Weirdly";
		if (base.CurJob.jType == JobType.PrisonerBeatingMild)
		{
			text = "Mildly";
		}
		if (base.CurJob.jType == JobType.PrisonerBeatingVicious)
		{
			text = "Viciously";
		}
		return new JobReport(text + " beating " + base.Victim.Label + ".", JobReportOverlays.warden);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return Toils_General.ReserveTarget(pawn, base.VictimPawn, ReservationType.HumanInteraction);
		foreach (Toil item in base.NewToilList())
		{
			yield return item;
		}
		yield return Toils_Prisoner.TryRecruitPrisoner(pawn, base.VictimPawn);
	}
}
