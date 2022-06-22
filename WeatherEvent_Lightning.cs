using UnityEngine;

public class WeatherEvent_Lightning : WeatherEvent
{
	private const int FlashFadeInTicks = 2;

	private const int MinFlashDuration = 20;

	private const int MaxFlashDuration = 45;

	private const float FlashShadowDistance = 5f;

	private int age;

	private int duration;

	private Vector2 shadowVector;

	public override bool Expired => age > duration;

	public override SkyTarget OverrideSkyTarget
	{
		get
		{
			SkyTarget skyTarget = new SkyTarget(1f, 1f, SkyColors.LightningFlash);
			skyTarget.overrideShadowVector = true;
			skyTarget.shadowVector = shadowVector;
			skyTarget.saturation = 1.15f;
			return skyTarget;
		}
	}

	public override float OverrideSkyTargetLerpFactor => LightningBrightness;

	protected float LightningBrightness
	{
		get
		{
			if (age <= 2)
			{
				return (float)age / 2f;
			}
			return 1f - (float)age / (float)duration;
		}
	}

	public WeatherEvent_Lightning()
	{
		duration = Random.Range(20, 45);
		shadowVector = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 0f));
	}

	public override void FireEvent()
	{
	}

	public override void WeatherEventTick()
	{
		age++;
	}
}
