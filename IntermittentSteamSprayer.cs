using UnityEngine;

public class IntermittentSteamSprayer
{
	private const int MinTicksBetweenSprays = 500;

	private const int MaxTicksBetweenSprays = 2000;

	private const int MinSprayDuration = 200;

	private const int MaxSprayDuration = 500;

	private const float SprayThickness = 0.6f;

	private Thing parent;

	private int ticksUntilSpray = 500;

	private int sprayTicksLeft;

	public IntermittentSteamSprayer(Thing parent)
	{
		this.parent = parent;
	}

	public void SteamSprayerTick()
	{
		if (sprayTicksLeft > 0)
		{
			sprayTicksLeft--;
			if (Random.value < 0.6f)
			{
				MoteMaker.ThrowAirPuffUp(parent.TrueCenter(), AltitudeLayer.Overworld);
			}
			if (sprayTicksLeft <= 0)
			{
				ticksUntilSpray = Random.Range(500, 2001);
			}
		}
		else
		{
			ticksUntilSpray--;
			if (ticksUntilSpray <= 0)
			{
				sprayTicksLeft = Random.Range(200, 501);
			}
		}
	}
}
