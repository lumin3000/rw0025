internal class Projectile_ExplosiveMolotov : Projectile_Explosive
{
	protected override void Explode()
	{
		for (int i = 0; i < 5; i++)
		{
			IntVec3 intVec = base.Position + Gen.RadialPattern[i];
			LiquidFuel liquidFuel = Find.Grids.ThingAt<LiquidFuel>(intVec);
			if (liquidFuel != null)
			{
				liquidFuel.Refill();
			}
			else
			{
				ThingMaker.Spawn(EntityType.LiquidFuel, intVec);
			}
		}
		base.Explode();
	}
}
