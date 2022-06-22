public class BulletIncendiary : Bullet
{
	protected override void Impact(Thing hitThing)
	{
		base.Impact(hitThing);
		if (hitThing != null)
		{
			hitThing.TryIgnite(0.2f);
		}
		else
		{
			ThingMaker.Spawn(EntityType.LiquidFuel, base.Position);
			FireUtility.TryStartFireIn(base.Position, 0.2f);
		}
		MoteMaker.ThrowFlash(base.Position, "ShotFlash", 6f);
		MoteMaker.ThrowMicroSparks(base.Position.ToVector3Shifted());
	}
}
