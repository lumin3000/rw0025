using System.Collections.Generic;

public abstract class JobDriver_TalkTo : JobDriverToil
{
	protected const int NumSpeechesToSay = 5;

	protected Pawn Talkee => (Pawn)base.CurJob.targetA.thing;

	public JobDriver_TalkTo(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Talking to " + Talkee.characterName + ".", null);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return Toils_Prisoner.GotoPrisoner(pawn, Talkee);
		int numSpeechesSent = 0;
		yield return new Toil
		{
			tickAction = delegate
			{
				if (!pawn.talker.MouthBusyTalking)
				{
					if (numSpeechesSent >= 5)
					{
						BeginNextToil();
					}
					else if (!base.CurJob.Def.speechToGive.TrySendFromTo(pawn, Talkee))
					{
						EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						numSpeechesSent++;
						if (Talkee.Team == TeamType.Prisoner)
						{
							pawn.skills.Learn(SkillType.Social, 50f);
						}
					}
				}
			},
			defaultCompleteMode = ToilCompleteMode.Never
		};
	}
}
