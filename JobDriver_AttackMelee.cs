using System.Collections.Generic;

public class JobDriver_AttackMelee : JobDriverToil
{
	private int numMeleeAttacksLanded;

	protected Thing Victim => base.CurJob.targetA.thing;

	protected Pawn VictimPawn => (Pawn)base.CurJob.targetA.thing;

	protected MeleeAttackMode MeleeMode => base.CurJob.Def.meleeMode;

	public JobDriver_AttackMelee(Pawn pawn)
		: base(pawn)
	{
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref numMeleeAttacksLanded, "NumMeleeAttacksLanded", 0);
	}

	public override JobReport GetReport()
	{
		return new JobReport("Melee attacking " + Victim.Label + ".", null);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil
		{
			tickAction = delegate
			{
				if (!pawn.pather.moving && !pawn.natives.CanTouch(Victim))
				{
					pawn.pather.StartPathTowards(Victim);
				}
				else if (pawn.natives.CanTouch(Victim))
				{
					if (Victim is Pawn && VictimPawn.Incapacitated && !base.CurJob.killIncappedTarget)
					{
						EndJobWith(JobCondition.Succeeded);
					}
					else if (pawn.natives.TryMeleeAttack(Victim, MeleeMode))
					{
						numMeleeAttacksLanded++;
						if (numMeleeAttacksLanded >= base.CurJob.maxNumMeleeAttacks)
						{
							if (VictimPawn.Team == TeamType.Prisoner)
							{
								VictimPawn.prisoner.lastWardenVisitTime = Find.TickManager.tickCount;
							}
							EndJobWith(JobCondition.Succeeded);
						}
					}
				}
			},
			tickFailCondition = ToilTools.StandardTickFail(pawn, Victim),
			defaultCompleteMode = ToilCompleteMode.Never
		};
	}
}
