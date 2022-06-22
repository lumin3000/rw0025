using UnityEngine;

public class Verb_MeleeAttack : Verb
{
	private const int TargetCooldown = 50;

	public MeleeAttackMode mode;

	public Verb_MeleeAttack(MeleeAttackMode mode)
	{
		this.mode = mode;
		vDefNative = NativeVerbDefDatabase.VerbWithID(VerbID.Melee);
	}

	protected override bool TryShotSpecialEffect()
	{
		Pawn ownerPawn = base.OwnerPawn;
		if (ownerPawn.stances.FullBodyBusy)
		{
			return false;
		}
		Thing thing = currentTarget.thing;
		Pawn pawn = currentTarget.thing as Pawn;
		if (!CanHitTarget(thing))
		{
			Debug.LogWarning(string.Concat(ownerPawn, " meleed ", thing, " from out of melee position."));
		}
		ownerPawn.pather.StopDead();
		float num;
		if ((thing.def.eType == EntityType.Pawn && !pawn.Incapacitated && !pawn.IsInBed() && mode != MeleeAttackMode.MildBeating && mode != MeleeAttackMode.ViciousBeating) || 1 == 0)
		{
			if (ownerPawn.skills != null)
			{
				num = SkillTunings.MeleeHitChanceAtLevel[ownerPawn.skills.LevelOf(SkillType.Melee)];
				ownerPawn.skills.Learn(SkillType.Melee, 100f);
			}
			else
			{
				num = 0.8f;
			}
		}
		else
		{
			num = 1f;
		}
		bool result;
		if (Random.value < num)
		{
			result = true;
			int newAmount = 0;
			if (mode == MeleeAttackMode.LethalToIncap)
			{
				newAmount = ownerPawn.raceDef.meleeDamage;
			}
			if (mode == MeleeAttackMode.ViciousBeating)
			{
				newAmount = Random.Range(2, 9);
			}
			if (mode == MeleeAttackMode.MildBeating)
			{
				newAmount = Random.Range(1, 4);
			}
			DamageInfo d = new DamageInfo(DamageType.Bludgeon, newAmount, (thing.Position - ownerPawn.Position).ToVector3());
			thing.TakeDamage(d);
			if (thing.def.category == EntityCategory.Building)
			{
				GenSound.PlaySoundAt(thing.Position, GenSound.RandomClipInFolder("Impact/PunchBuilding"), 0.5f);
			}
			else
			{
				GenSound.PlaySoundAt(ownerPawn.Position, GenSound.RandomClipInFolder("Impact/PunchPawn"), 0.5f);
			}
			ThoughtType thoughtType = ThoughtType.Undefined;
			ThoughtType thoughtType2 = ThoughtType.Undefined;
			if (mode == MeleeAttackMode.MildBeating)
			{
				thoughtType = ThoughtType.PrisonerBeatenMild;
				thoughtType2 = ThoughtType.KnowPrisonerBeaten;
			}
			if (mode == MeleeAttackMode.ViciousBeating)
			{
				thoughtType = ThoughtType.PrisonerBeatenVicious;
				thoughtType2 = ThoughtType.KnowPrisonerBeaten;
			}
			if (thoughtType != 0)
			{
				pawn.psychology.thoughts.GainThought(thoughtType);
			}
			if (thoughtType2 != 0)
			{
				foreach (Pawn colonistsAndPrisoner in Find.PawnManager.ColonistsAndPrisoners)
				{
					if (colonistsAndPrisoner != pawn)
					{
						colonistsAndPrisoner.psychology.thoughts.GainThought(thoughtType2);
					}
				}
			}
		}
		else
		{
			result = false;
			GenSound.PlaySoundAt(ownerPawn.Position, GenSound.RandomClipInFolder("Impact/PunchMiss"), 0.5f);
		}
		ownerPawn.drawer.Notify_MeleeAttackOn(thing);
		if (pawn != null)
		{
			Stance_Cooldown stance_Cooldown = pawn.stances.curStance as Stance_Cooldown;
			if (stance_Cooldown == null || stance_Cooldown.stanceTicksLeft >= 50)
			{
				pawn.stances.SetStance(new Stance_Cooldown(50));
			}
			if (ownerPawn.Team != TeamType.Colonist || pawn.Team != TeamType.Prisoner)
			{
				pawn.MindState.closeThreat = ownerPawn;
				pawn.MindState.lastCloseThreatHarmTime = Find.TickManager.tickCount;
				if (pawn.jobs.CurJob != null && pawn.jobs.CurJob.Def.easyInterrupt && (pawn.MindHuman == null || !pawn.MindHuman.drafted))
				{
					pawn.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
				}
			}
		}
		ownerPawn.drawer.rotator.FaceSquare(thing.Position);
		return result;
	}
}
