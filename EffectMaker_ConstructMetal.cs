using System.Collections.Generic;
using UnityEngine;

public class EffectMaker_ConstructMetal : EffectMaker
{
	private const int SoundInterval = 55;

	private const float SoundVolume = 0.1f;

	private const float BaseDustChance = 0.035f;

	private const float SparkChance = 0.2f;

	protected int ticksUntilSound;

	public override void EffectTick(Thing A, Thing B)
	{
		float num = 0.035f;
		num *= (float)(B.def.size.x * B.def.size.z);
		if (Random.value < num)
		{
			MoteMaker.ThrowDustPuff(new List<IntVec3>(Gen.SquaresOccupiedBy(B)).RandomElement(), 1f);
		}
		if (Random.value < 0.2f)
		{
			MoteMaker.ThrowSpark(A.Position, B.Position);
		}
		ticksUntilSound--;
		if (ticksUntilSound <= 0)
		{
			AudioClip clip = GenSound.RandomClipInFolder("Interaction/Construction", NoRepeat: true);
			GenSound.PlaySoundAt(A.Position, clip, 0.1f);
			ticksUntilSound = 55;
		}
	}
}
