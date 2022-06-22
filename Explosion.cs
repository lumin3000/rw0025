using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Explosion
{
	private const int StandardBombDamage = 65;

	private const int StandardFlameDamage = 10;

	private const int StandardIceDamage = 35;

	public const float BombBuildingDamageMultiplier = 10f;

	private const float CamShakeMultiplier = 4f;

	public IntVec3 center;

	public float radius;

	public DamageInfo damage;

	private static readonly AudioClip ExplosionSoundBomb = Res.LoadSound("Explosion/ExplosionBomb");

	private static readonly AudioClip ExplosionSoundFire = Res.LoadSound("Explosion/ExplosionFireA");

	public Explosion(IntVec3 Center, float Radius, DamageInfo Damage)
	{
		center = Center;
		radius = Radius;
		damage = Damage;
	}

	public static void DoExplosion(IntVec3 Loc, float radius, DamageType damType)
	{
		int newAmount = 0;
		if (damType == DamageType.Bomb)
		{
			newAmount = 65;
		}
		if (damType == DamageType.Flame)
		{
			newAmount = 10;
		}
		if (damType == DamageType.Stun)
		{
			newAmount = 35;
		}
		Explosion explosion = new Explosion(Loc, radius, new DamageInfo(damType, newAmount));
		explosion.Explode();
	}

	public static void DisplayPredictedExplosiveRadius(IntVec3 loc, float radius)
	{
		Explosion explosion = new Explosion(loc, radius, new DamageInfo(DamageType.Bomb, 65));
		List<IntVec3> fieldSquares = explosion.SquaresToHit().ToList();
		GenRender.RenderFieldEdges(fieldSquares);
	}

	public void Explode()
	{
		List<IntVec3> list = SquaresToHit().ToList();
		foreach (IntVec3 item in list)
		{
			AffectSquare(item);
		}
		float magnitude = (center.ToVector3() - Find.CameraMap.transform.position).magnitude;
		Find.CameraMap.shaker.DoShake(4f * radius / magnitude);
		MoteMaker.ThrowFlash(center, "ExplosionFlash", radius * 6f);
		for (int i = 0; i < 4; i++)
		{
			MoteMaker.ThrowSmoke(center.ToVector3Shifted() + Gen.RandomHorizontalVector(radius * 0.7f), radius * 0.6f);
		}
		AudioClip clip = null;
		if (damage.type == DamageType.Bomb)
		{
			clip = ExplosionSoundBomb;
		}
		if (damage.type == DamageType.Stun)
		{
			clip = ExplosionSoundBomb;
		}
		if (damage.type == DamageType.Flame)
		{
			clip = ExplosionSoundFire;
		}
		GenSound.PlaySoundAt(center, clip, 1f);
	}

	private IEnumerable<IntVec3> SquaresToHit()
	{
		List<IntVec3> list = new List<IntVec3>();
		int num = Gen.NumSquaresInRadius(radius);
		for (int i = 0; i < num; i++)
		{
			IntVec3 intVec = center + Gen.RadialPattern[i];
			if (intVec.InBounds() && GenGrid.LineOfSight(center, intVec, skipFirstSquare: true))
			{
				list.Add(intVec);
			}
		}
		List<IntVec3> list2 = new List<IntVec3>();
		foreach (IntVec3 item in list)
		{
			if (!item.Walkable())
			{
				continue;
			}
			foreach (IntVec3 item2 in item.AdjacentSquaresCardinal())
			{
				if (item2.WithinHorizontalDistanceOf(center, radius) && item2.InBounds() && !item2.Standable() && Find.Grids.BlockerAt(item2) != null)
				{
					list2.Add(item2);
				}
			}
		}
		return list.Concat(list2).Distinct();
	}

	private void AffectSquare(IntVec3 sq)
	{
		MoteMaker.ThrowExplosionSquare(sq);
		foreach (Thing item in Find.Grids.ThingsAt(sq).ListFullCopy())
		{
			DoDamageTo(item);
		}
		if (damage.type == DamageType.Flame)
		{
			FireUtility.TryStartFireIn(sq, UnityEngine.Random.Range(0.2f, 0.6f));
		}
	}

	private void DoDamageTo(Thing t)
	{
		if (damage.type == DamageType.Bomb && t.def.eType == EntityType.Fire)
		{
			t.Destroy();
			return;
		}
		DamageInfo damageInfo = new DamageInfo(damage);
		if (t.Position == center)
		{
			damageInfo.direction = UnityEngine.Random.Range(0, 360);
		}
		else
		{
			damageInfo.direction = (t.Position - center).AngleFlat;
		}
		if (damageInfo.type == DamageType.Bomb && t.def.category == EntityCategory.Building)
		{
			int newAmount = (int)Math.Round((float)damageInfo.Amount * 10f);
			damageInfo = new DamageInfo(damage.type, newAmount, damageInfo.direction);
		}
		t.TakeDamage(damageInfo);
	}
}
