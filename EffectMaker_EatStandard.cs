using UnityEngine;

public class EffectMaker_EatStandard : EffectMaker
{
	private const int TicksBetweenParticles = 20;

	private int ticksUntilParticle;

	public override void EffectTick(Thing A, Thing B)
	{
		ticksUntilParticle--;
		if (ticksUntilParticle <= 0)
		{
			Vector3 spawnLoc = A.DrawPos * 0.35f + B.DrawPos * 0.65f;
			MoteMaker.ThrowFoodBit(spawnLoc);
			ticksUntilParticle = 20;
		}
	}
}
