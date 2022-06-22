using System;
using UnityEngine;

public class Mineable : Building, Interactive
{
	public const int BaseTicksBetweenPickHits = 100;

	private const int BaseDamagePerPickHit = 80;

	private const int ResourceAmountOnSpawn = 35;

	private const float NonMiningDamageEfficiency = 0.8f;

	private int ticksToPickHit = -1000;

	private int damageTakenNotMining;

	private static readonly AudioClip CollapseSound = Res.LoadSound("Building/RockCollapse");

	private int DamagePerPickHit
	{
		get
		{
			int num = 80;
			if (Find.ResearchManager.HasResearched(ResearchType.PneumaticPicks))
			{
				num = (int)Math.Round((float)num * 1.2f);
			}
			return num;
		}
	}

	public JobCondition InteractedWith(ReservationType WType, Pawn pawn)
	{
		if (ticksToPickHit < -100)
		{
			ResetTicksToPickHit(pawn);
		}
		ticksToPickHit--;
		if (ticksToPickHit <= 0)
		{
			PickHitFrom(pawn);
			ResetTicksToPickHit(pawn);
		}
		if (destroyed)
		{
			return JobCondition.Succeeded;
		}
		return JobCondition.Ongoing;
	}

	private void ResetTicksToPickHit(Pawn pawn)
	{
		float num = 0.5f + 0.15f * (float)pawn.skills.LevelOf(SkillType.Mining);
		num *= pawn.healthTracker.CurEffectivenessPercent;
		ticksToPickHit = (int)Math.Round(100f / num);
	}

	protected void PickHitFrom(Pawn p)
	{
		GenSound.PlaySoundAt(base.Position, GenSound.RandomClipInFolder("Interaction/PickHit"), 0.15f);
		MoteMaker.MakeSpark(p, base.Position);
		DamageInfo d = new DamageInfo(DamageType.Mining, DamagePerPickHit);
		TakeDamage(d);
		if (destroyed)
		{
			Find.TerrainGrid.SetTerrain(base.Position, TerrainDefDatabase.TerrainWithLabel("Rough-hewn rock"));
		}
		if (p.skills != null)
		{
			p.skills.Learn(SkillType.Mining, 13f);
		}
	}

	protected override void ApplyDamage(DamageInfo damage)
	{
		if (damage.type.HarmsHealth() && damage.type != DamageType.Mining)
		{
			int num = Math.Min(damage.Amount, health);
			damageTakenNotMining += num;
		}
		base.ApplyDamage(damage);
	}

	public override void Killed(DamageInfo dinfo)
	{
		GenSound.PlaySoundAt(base.Position, CollapseSound, 0.05f);
		base.Killed(dinfo);
		SpawnResource(base.Position);
	}

	protected void SpawnResource(IntVec3 SpawnLoc)
	{
		if (def.mineableResource != 0)
		{
			ThingDefinition thingDefinition = def.mineableResource.DefinitionOfType();
			Thing thing = ThingMaker.MakeThing(thingDefinition);
			float num = 0.8f + 0.19999999f * (1f - (float)damageTakenNotMining / (float)def.maxHealth);
			thing.stackCount = (int)Math.Ceiling(35f * num);
			ThingMaker.Spawn(thing, SpawnLoc);
		}
	}
}
