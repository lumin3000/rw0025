using System;
using UnityEngine;

public class Verb_LaunchProjectile : Verb
{
	protected override bool TryShotSpecialEffect()
	{
		ShootLine shootLine = ShootLineFromTo(owner.Position, currentTarget);
		if (!shootLine.found)
		{
			return false;
		}
		Vector3 drawPos = owner.DrawPos;
		Projectile projectile = (Projectile)ThingMaker.MakeThing(base.VerbDef.projDef);
		ThingMaker.Spawn(projectile, shootLine.source);
		float num = projectile.def.projectile_RandomMissRadius;
		float lengthHorizontalSquared = (currentTarget.Loc - owner.Position).LengthHorizontalSquared;
		if (lengthHorizontalSquared < 9f)
		{
			num = 0f;
		}
		else if (lengthHorizontalSquared < 25f)
		{
			num *= 0.5f;
		}
		else if (lengthHorizontalSquared < 49f)
		{
			num *= 0.8f;
		}
		if (num > 0.1f)
		{
			int max = Gen.NumSquaresInRadius(projectile.def.projectile_RandomMissRadius);
			int num2 = UnityEngine.Random.Range(0, max);
			IntVec3 intVec = currentTarget.Loc + Gen.RadialPattern[num2];
			projectile.canFreeIntercept = true;
			projectile.Launch(drawPos, intVec);
			return true;
		}
		HitReport hitReport = HitReportFor(currentTarget);
		if (UnityEngine.Random.value > hitReport.TotalNonWildShotChance)
		{
			shootLine.ChangeDestToMissWild();
			projectile.canFreeIntercept = true;
			projectile.Launch(drawPos, shootLine.dest);
			return true;
		}
		if (UnityEngine.Random.value > hitReport.HitChanceThroughCover && currentTarget.thing != null && currentTarget.thing.def.eType == EntityType.Pawn)
		{
			Thing targetThing = hitReport.covers.RandomBlockingCoverWeighted();
			projectile.canFreeIntercept = true;
			projectile.Launch(drawPos, new TargetPack(targetThing));
			return true;
		}
		if (currentTarget.thing != null)
		{
			projectile.Launch(drawPos, new TargetPack(currentTarget.thing));
		}
		else
		{
			projectile.Launch(drawPos, new TargetPack(shootLine.dest));
		}
		return true;
	}

	public HitReport HitReportFor(TargetPack target)
	{
		IntVec3 loc = target.Loc;
		HitReport hitReport = new HitReport();
		hitReport.shotDistance = (loc - owner.Position).LengthHorizontal;
		hitReport.target = target;
		if (!base.VerbDef.canMiss)
		{
			hitReport.hitChanceThroughSkill = 1f;
			hitReport.covers = new CoverUtility.CoverGiverSet();
		}
		else
		{
			float num = 0.98f;
			if (base.OwnerIsPawn)
			{
				num = SkillTunings.AccuracyAtLevel[base.OwnerPawn.skills.LevelOf(SkillType.Shooting)];
			}
			hitReport.hitChanceThroughSkill = (float)Math.Pow(num, hitReport.shotDistance);
			if (hitReport.hitChanceThroughSkill < 0.0201f)
			{
				hitReport.hitChanceThroughSkill = 0.0201f;
			}
			hitReport.hitChanceThroughEquipment = (float)Math.Pow(base.VerbDef.hitMultiplierPerDist, hitReport.shotDistance);
			if (hitReport.hitChanceThroughEquipment < 0.0201f)
			{
				hitReport.hitChanceThroughEquipment = 0.0201f;
			}
			hitReport.covers = CoverUtility.CoverGiverSetAtFrom(loc, owner.Position);
			hitReport.targetLighting = Find.GlowGrid.PsychGlowAt(loc);
			hitReport.hitChanceThroughWeather = Find.WeatherManager.CurWeatherAccuracyMultiplier;
			if (target.HasThing && target.thing is Pawn)
			{
				hitReport.hitChanceThroughTargetSize = ((Pawn)(Thing)target).raceDef.targetHitEase;
			}
		}
		return hitReport;
	}

	public override float HighlightFieldRadiusAroundTarget()
	{
		return base.VerbDef.projDef.projectile_ExplosionRadius;
	}
}
