using System.Collections.Generic;
using UnityEngine;

public class EffectMaker_ConstructDig : EffectMaker
{
	private const int SoundInterval = 100;

	private const float SoundVolume = 0.12f;

	private const float BaseDustChance = 0.035f;

	protected int ticksUntilSound;

	public override void EffectTick(Thing A, Thing B)
	{
		float num = 0.035f;
		num *= (float)(B.def.size.x * B.def.size.z);
		if (Random.value < num)
		{
			MoteMaker.ThrowDustPuff(new List<IntVec3>(Gen.SquaresOccupiedBy(B)).RandomElement(), 1f);
		}
		ticksUntilSound--;
		if (ticksUntilSound <= 0)
		{
			AudioClip clip = GenSound.RandomClipInFolder("Interaction/Digging", NoRepeat: true);
			GenSound.PlaySoundAt(A.Position, clip, 0.12f);
			ticksUntilSound = 100;
		}
	}
}
