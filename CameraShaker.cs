using System;
using UnityEngine;

public class CameraShaker
{
	private const float ShakeDecayRate = 0.5f;

	private const float ShakeFrequency = 24f;

	private const float MaxShakeMag = 0.1f;

	private float curShakeMag;

	public Vector3 ShakeOffset
	{
		get
		{
			float x = (float)Math.Sin(Find.RealTime.timeUnpaused * 24f) * curShakeMag;
			float y = (float)Math.Sin((double)(Find.RealTime.timeUnpaused * 24f) * 1.05) * curShakeMag;
			float z = (float)Math.Sin((double)(Find.RealTime.timeUnpaused * 24f) * 1.1) * curShakeMag;
			return new Vector3(x, y, z);
		}
	}

	public void DoShake(float mag)
	{
		curShakeMag += mag;
		if (curShakeMag > 0.1f)
		{
			curShakeMag = 0.1f;
		}
	}

	public void Update()
	{
		curShakeMag -= 0.5f * Time.deltaTime;
		if (curShakeMag < 0f)
		{
			curShakeMag = 0f;
		}
	}
}
