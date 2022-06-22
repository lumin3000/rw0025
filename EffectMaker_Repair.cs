using UnityEngine;

public class EffectMaker_Repair : EffectMaker
{
	private const float SparkChance = 0.1f;

	private const int TicksBeforeLoopStart = 35;

	protected bool SoundInitiated;

	protected int TicksUntilLoopStart;

	protected SoundLooper WeldLooper;

	public override void EffectTick(Thing A, Thing B)
	{
		if (Random.value < 0.1f)
		{
			MoteMaker.ThrowSpark(A.Position, B.Position);
		}
		if (!SoundInitiated)
		{
			TicksUntilLoopStart = 35;
			GenSound.PlaySoundAt(A.Position, "Interaction/Repair/WeldStart", 0.18f);
			SoundInitiated = true;
			return;
		}
		if (TicksUntilLoopStart > 0)
		{
			TicksUntilLoopStart--;
			if (TicksUntilLoopStart == 0)
			{
				WeldLooper = new SoundLooperThing(A, (AudioClip)Resources.Load("Sounds/Interaction/Repair/WeldLoop"), 0.25f, SoundLooperMaintenanceType.PerTick);
			}
		}
		if (WeldLooper != null)
		{
			WeldLooper.Maintain();
		}
	}
}
