public class EffectMaker_Research : EffectMaker
{
	protected const float ProgressBetweenSounds = 750f;

	protected float ticksToNextEffect;

	public override void EffectTick(Thing A, Thing B)
	{
		ticksToNextEffect -= 1f;
		if (ticksToNextEffect <= 0f)
		{
			GenSound.PlaySoundAt(B.Position, GenSound.RandomClipInFolder("Interaction/Researching"), 0.3f);
			for (int i = 0; i < 3; i++)
			{
				MoteMaker.ThrowAirPuffUp(B.Position.ToVector3Shifted(), AltitudeLayer.HighMote);
			}
			ticksToNextEffect = 750f;
		}
	}
}
