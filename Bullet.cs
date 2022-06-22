using UnityEngine;

public class Bullet : Projectile
{
	private const float ImpactSoundVolume = 0.2f;

	private const float StunChance = 0.1f;

	protected override void Impact(Thing hitThing)
	{
		base.Impact(hitThing);
		if (hitThing != null)
		{
			int projectile_DamageAmountBase = def.projectile_DamageAmountBase;
			DamageInfo d = new DamageInfo(DamageType.Bullet, projectile_DamageAmountBase, ExactRotation.eulerAngles.y);
			hitThing.TakeDamage(d);
			if (hitThing.def.bulletHitSoundFolder != string.Empty)
			{
				GenSound.PlaySoundAt(base.Position, GenSound.RandomClipInFolder("Impact/" + hitThing.def.bulletHitSoundFolder), 0.2f);
			}
			Pawn pawn = hitThing as Pawn;
			if (pawn != null && !pawn.Incapacitated && Random.value < 0.1f)
			{
				hitThing.TakeDamage(new DamageInfo(DamageType.Stun, 10));
			}
		}
		else
		{
			GenSound.PlaySoundAt(base.Position, GenSound.RandomClipInFolder("Impact/Ground"), 0.2f);
			MoteMaker.MakeShotHitDirt(ExactPosition);
		}
	}
}
