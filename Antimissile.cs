using UnityEngine;

public class Antimissile : Projectile
{
	public override Vector3 ExactPosition
	{
		get
		{
			Vector3 vector = (((Projectile)assignedTarget).ExactPosition - origin) * (1f - (float)ticksToImpact / (float)base.StartingTicksToImpact);
			return origin + vector + Vector3.up * def.altitude;
		}
	}

	public override Quaternion ExactRotation => Quaternion.LookRotation(((Projectile)assignedTarget).ExactPosition - ExactPosition);

	protected override void Impact(Thing hitThing)
	{
		base.Impact(hitThing);
		hitThing.Destroy();
	}
}
