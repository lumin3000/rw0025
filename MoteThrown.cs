using UnityEngine;

public class MoteThrown : Mote
{
	public int airTicksLeft = 9999;

	public float exactVelocity;

	public float exactMoveDirection;

	protected bool Flying => airTicksLeft > 0;

	protected bool Skidding => !Flying && exactVelocity > 0.01f;

	public override void Tick()
	{
		base.Tick();
		if (!Flying && !Skidding)
		{
			return;
		}
		Vector3 vector = exactMoveDirection.ToQuat() * Vector3.forward * exactVelocity;
		IntVec3 intVec = new IntVec3(exactPosition + vector);
		if (intVec != base.Position)
		{
			if (!intVec.InBounds())
			{
				Destroy();
				return;
			}
			if (def.mote.useCollision && !intVec.Standable())
			{
				WallHit();
				return;
			}
		}
		base.Position = intVec;
		exactPosition += vector;
		exactRotation += exactRotationRate;
		if (airTicksLeft > 0)
		{
			airTicksLeft--;
			if (airTicksLeft == 0)
			{
				GroundHit();
			}
		}
		if (Skidding)
		{
			exactVelocity *= skidSpeedMultiplierPerTick;
			exactRotationRate *= skidSpeedMultiplierPerTick;
			if (exactVelocity < 0.02f)
			{
				exactVelocity = 0f;
			}
		}
	}

	protected virtual void WallHit()
	{
		airTicksLeft = 0;
		exactVelocity = 0f;
		exactRotationRate = 0f;
		GroundHit();
	}

	protected virtual void GroundHit()
	{
	}
}
