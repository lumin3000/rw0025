using System;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_HealthTracker : Saveable
{
	private const int TicksBetweenNaturalHealsInBed = 1000;

	private const float IncapPercent = 0.4f;

	private const int IncapThreshMax = 40;

	private const int TicksBetweenBleeds = 750;

	private const float IncapDamageMultiplier = 0.25f;

	private const float DeathWitnessRadius = 18f;

	private Pawn pawn;

	private bool incapacitated;

	private int airStarveTicks;

	private int ticksToNextBleed = 750;

	private int ticksToNextHeal = 1000;

	public bool forceIncap;

	private static readonly AudioClip IncapSound = Res.LoadSound("Interaction/Haul/CorpseDrop");

	private static int BleedBloodSpawnInterval = 2;

	public int Health
	{
		get
		{
			return pawn.health;
		}
		set
		{
			pawn.health = value;
		}
	}

	public int BaseMaxHealth => pawn.raceDef.baseMaxHealth;

	public int MaxHealth
	{
		get
		{
			int num = BaseMaxHealth;
			if (pawn.traits.HasTraitEffect(TraitEffect.MoreHealth))
			{
				num += 50;
			}
			return num;
		}
	}

	public bool Wounded => Health < MaxHealth;

	public bool Incapacitated => incapacitated;

	private int IncapThreshold
	{
		get
		{
			int num = (int)Math.Round((float)MaxHealth * 0.4f);
			if (num > 40)
			{
				return 40;
			}
			return num;
		}
	}

	public EffectivenessClass CurEffectiveness
	{
		get
		{
			if (Incapacitated)
			{
				return EffectivenessClass.Incapacitated;
			}
			int num = 0;
			if ((float)Health < (float)MaxHealth * 0.9f)
			{
				num++;
			}
			if ((float)Health < (float)MaxHealth * 0.7f)
			{
				num++;
			}
			if ((float)Health < (float)MaxHealth * 0.55f)
			{
				num++;
			}
			if (pawn.rest != null)
			{
				if (pawn.rest.Rest.CurFatigueLevel == FatigueLevel.VeryTired)
				{
					num += 2;
				}
				if (pawn.rest.Rest.CurFatigueLevel == FatigueLevel.Exhausted)
				{
					num += 3;
				}
			}
			if (pawn.food != null)
			{
				if (pawn.food.Food.CurHungerLevel == HungerLevel.UrgentlyHungry)
				{
					num += 2;
				}
				if (pawn.food.Food.CurHungerLevel == HungerLevel.Starving)
				{
					num += 3;
				}
			}
			if (num <= 2)
			{
				return EffectivenessClass.Full;
			}
			if (num <= 4)
			{
				return EffectivenessClass.Reduced;
			}
			return EffectivenessClass.BadlyImpaired;
		}
	}

	public float CurEffectivenessPercent
	{
		get
		{
			switch (CurEffectiveness)
			{
			case EffectivenessClass.Full:
				return 1f;
			case EffectivenessClass.Reduced:
				return 0.8f;
			case EffectivenessClass.BadlyImpaired:
				return 0.6f;
			case EffectivenessClass.Incapacitated:
				return 0f;
			default:
				Debug.LogWarning("Effectiveness needs percent.");
				return 1f;
			}
		}
	}

	public Pawn_HealthTracker(Pawn newPawn)
	{
		pawn = newPawn;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref pawn.health, "PawnHealth", forceSave: true);
		Scribe.LookField(ref incapacitated, "Incapacitated");
		Scribe.LookField(ref airStarveTicks, "AirStarveTicks", 0);
		Scribe.LookField(ref ticksToNextBleed, "TicksToNextBleed", 750);
		Scribe.LookField(ref ticksToNextHeal, "TicksToNextHeal", 1000);
		Scribe.LookField(ref forceIncap, "ForceIncap", defaultValue: false, forceSave: false);
	}

	public void HealthTick()
	{
		if (pawn.raceDef.canBleed && (float)Health < (float)MaxHealth * 0.5f && UnityEngine.Random.value < 0.003f * (float)(Health / MaxHealth))
		{
			TryDropBloodFilth();
		}
		if (pawn.IsInBed())
		{
			return;
		}
		if (Incapacitated)
		{
			ticksToNextBleed--;
			if (ticksToNextBleed <= 0)
			{
				ticksToNextBleed = 750;
				pawn.TakeDamage(new DamageInfo(DamageType.Bleeding, 1));
				if (pawn.health % BleedBloodSpawnInterval == 0)
				{
					TryDropBloodFilth();
				}
			}
		}
		else if (Health < MaxHealth && (pawn.psychology == null || !pawn.food.Food.Starving))
		{
			ticksToNextHeal--;
			if (ticksToNextHeal <= 0)
			{
				ticksToNextHeal = 1000;
				pawn.TakeDamage(new DamageInfo(DamageType.Healing, 1));
			}
		}
	}

	public void ApplyDamage(DamageInfo dInfo)
	{
		if (!DebugSettings.playerDamageEnabled && pawn.Team == TeamType.Colonist)
		{
			return;
		}
		if (dInfo.type == DamageType.Flame && pawn.Team == TeamType.Colonist)
		{
			Find.TickManager.slower.SignalForceNormalShort();
		}
		int amount = dInfo.Amount;
		if (Incapacitated && dInfo.type.HarmsHealth())
		{
			amount = Gen.Clamp((int)((float)amount * 0.25f), 1, 999999);
		}
		if (dInfo.type == DamageType.Stun)
		{
			pawn.stances.stunner.Notify_DamageApplied(dInfo);
		}
		else if (dInfo.type == DamageType.Healing)
		{
			Health = Gen.Clamp(Health + dInfo.Amount, 0, MaxHealth);
		}
		else
		{
			Health = Gen.Clamp(Health - dInfo.Amount, 0, MaxHealth);
			if (pawn.psychology != null && (dInfo.type == DamageType.Bullet || dInfo.type == DamageType.Bomb || dInfo.type == DamageType.Bludgeon))
			{
				pawn.psychology.thoughts.GainThought(ThoughtType.BattleWounded);
			}
		}
		if (dInfo.type.MakesBlood())
		{
			TryDropBloodFilth();
		}
		if (Health <= 0)
		{
			PawnKilled(dInfo);
			return;
		}
		if (dInfo.type == DamageType.Flame)
		{
			pawn.TryIgnite(UnityEngine.Random.Range(0.15f, 0.25f));
		}
		if (pawn.raceDef.humanoid && !Incapacitated && Health < IncapThreshold && dInfo.type.HarmsHealth())
		{
			int num = Health + dInfo.Amount;
			int num2 = dInfo.Amount;
			if (num2 > IncapThreshold - num)
			{
				num2 = IncapThreshold - num;
			}
			float num3 = 1f - Mathf.Pow(1f - pawn.kindDef.baseIncapChancePerDamage, num2);
			num3 *= dInfo.type.IncapChanceMultiplier();
			float num4;
			if (pawn.Team == TeamType.Colonist)
			{
				num4 = 1f;
			}
			else
			{
				num4 = Find.Storyteller.intenderPopulation.PopulationIntent;
				if (num4 < 0.25f)
				{
					num4 = 0.25f;
				}
			}
			num3 *= num4;
			if (DebugSettings.logIncapChance)
			{
				UI_Messages.Message(string.Concat(pawn, " incapChance:", (num3 * 100f).ToString("###0.0"), " (Damage=", dInfo.Amount, " UnderThresh=", num2, ")"), UIMessageSound.Silent);
			}
			if (UnityEngine.Random.value < num3 || forceIncap)
			{
				NewlyIncapacitated();
				return;
			}
		}
		if (Incapacitated && Health > IncapThreshold && dInfo.type == DamageType.Healing)
		{
			NewlyCapacitated();
		}
	}

	private void PawnKilled(DamageInfo Damage)
	{
		if (pawn.carrier != null)
		{
			pawn.carrier.carryHands.DropCarriedThing();
		}
		pawn.IncappedOrKilled();
		pawn.Destroy();
		if (pawn.raceDef.humanoid)
		{
			ThoughtUtility.BroadcastThought(ThoughtType.WitnessedDeath, pawn.Position, 18f);
		}
		if (pawn.Team == TeamType.Colonist)
		{
			if (Damage.type == DamageType.Bleeding)
			{
				UI_Messages.Message(pawn.Label + " has succumbed to " + pawn.Possessive() + " wounds.", UIMessageSound.Negative);
			}
			else if (Damage.type == DamageType.Starvation)
			{
				UI_Messages.Message(pawn.Label + " has starved to death.", UIMessageSound.Negative);
			}
			else if (Damage.type == DamageType.Suffocation)
			{
				UI_Messages.Message(pawn.Label + " has suffocated to death.", UIMessageSound.Negative);
			}
			else
			{
				UI_Messages.Message(pawn.Label + " has been killed.", UIMessageSound.Negative);
			}
		}
		Corpse corpse = ThingMaker.MakeThing<Corpse>();
		corpse.sourcePawn = pawn;
		ThingMaker.Spawn(corpse, pawn.Position, pawn.rotation);
		corpse.SetForbidden(value: true);
		if (pawn.raceDef.deathAction != null)
		{
			pawn.raceDef.deathAction(pawn);
		}
	}

	private void NewlyIncapacitated()
	{
		if (incapacitated)
		{
			Debug.LogError(string.Concat(pawn, " was incapacitated more than once."));
			return;
		}
		pawn.pather.StopDead();
		incapacitated = true;
		ticksToNextBleed = 750;
		pawn.IncappedOrKilled();
		if (pawn.GetKing() != null)
		{
			pawn.GetKing().fleeChecker.Notify_PawnIncapped(pawn);
		}
		pawn.stances.CancelActionIfPossible();
		GenSound.PlaySoundAt(pawn.Position, IncapSound);
		if (pawn.MindHuman != null && pawn.MindHuman.drafted)
		{
			pawn.MindHuman.drafted = false;
		}
		if (pawn.Team.IsHostileToTeam(TeamType.Colonist))
		{
			Find.Tutor.Signal(TutorSignal.EnemyIncapacitated, pawn);
		}
		if (pawn.Team == TeamType.Colonist)
		{
			Find.Tutor.Signal(TutorSignal.ColonistIncapacitated, pawn);
		}
	}

	private void NewlyCapacitated()
	{
		if (!incapacitated)
		{
			Debug.LogError(string.Concat(pawn, " was un-incapacitated more than once."));
			return;
		}
		incapacitated = false;
		if (pawn.Team == TeamType.Colonist)
		{
			UI_Messages.Message(pawn.Label + " is no longer incapacitated.", UIMessageSound.Benefit);
		}
	}

	public void Heal(int Amount)
	{
		Health += Amount;
		if (Health > MaxHealth)
		{
			Health = MaxHealth;
		}
	}

	private void TryDropBloodFilth()
	{
		if (pawn.raceDef.canBleed && UnityEngine.Random.value < 0.5f)
		{
			ThingDefinition filthDef = ThingDefDatabase.ThingDefNamed("Blood");
			List<string> list = new List<string>();
			list.Add(pawn.Label);
			if (pawn.carrier == null)
			{
				FilthUtility.AddFilthAt(pawn.Position, filthDef, list);
			}
			else
			{
				pawn.carrier.filth.GainFilth(filthDef, list);
			}
		}
	}

	public void ForceIncap()
	{
		pawn.health = IncapThreshold - UnityEngine.Random.Range(1, 5);
		if (pawn.spawnedInWorld)
		{
			NewlyIncapacitated();
		}
		else
		{
			incapacitated = true;
		}
	}
}
