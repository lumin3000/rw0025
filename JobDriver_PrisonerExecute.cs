using System.Collections.Generic;

public class JobDriver_PrisonerExecute : JobDriverToil
{
	protected Pawn Prisoner => (Pawn)base.CurJob.targetA.thing;

	public JobDriver_PrisonerExecute(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Executing prisoner " + Prisoner.characterName + ".", JobReportOverlays.warden);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return Toils_General.ReserveTarget(pawn, Prisoner, ReservationType.HumanInteraction);
		yield return Toils_Prisoner.GotoPrisoner(pawn, Prisoner);
		yield return new Toil
		{
			initAction = delegate
			{
				Prisoner.TakeDamage(new DamageInfo(DamageType.Bludgeon, 999));
				foreach (Pawn colonistsAndPrisoner in Find.PawnManager.ColonistsAndPrisoners)
				{
					colonistsAndPrisoner.psychology.thoughts.GainThought(ThoughtType.KnowPrisonerExecuted);
				}
			},
			defaultCompleteMode = ToilCompleteMode.Immediate
		};
	}
}
