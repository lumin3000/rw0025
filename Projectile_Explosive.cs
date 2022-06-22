public class Projectile_Explosive : Projectile
{
	private int ticksToDetonation;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref ticksToDetonation, "TicksToDetonation");
	}

	public override void Tick()
	{
		base.Tick();
		if (ticksToDetonation > 0)
		{
			ticksToDetonation--;
			if (ticksToDetonation <= 0)
			{
				Explode();
			}
		}
	}

	protected override void Impact(Thing HitThing)
	{
		if (def.projectile_ExplosionDelay == 0)
		{
			Explode();
			return;
		}
		landed = true;
		ticksToDetonation = def.projectile_ExplosionDelay;
	}

	protected virtual void Explode()
	{
		Destroy();
		Explosion.DoExplosion(base.Position, def.projectile_ExplosionRadius, def.projectile_DamageType);
	}
}
