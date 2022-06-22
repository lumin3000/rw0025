using System.Collections.Generic;

public class JobDriver_TalkToPrisoner : JobDriver_TalkTo
{
	public JobDriver_TalkToPrisoner(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Chatting with prisoner " + base.Talkee.Label + ".", JobReportOverlays.warden);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		foreach (Toil item in base.NewToilList())
		{
			yield return item;
		}
		yield return Toils_Prisoner.TryRecruitPrisoner(pawn, base.Talkee);
	}
}
