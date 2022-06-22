using System;
using UnityEngine;

public static class Altitudes
{
	private const float AltitudeSpacing = 0.3f;

	public const float AltInc = 0.04f;

	private static readonly float[] Alts;

	public static readonly Vector3 AltIncVect;

	static Altitudes()
	{
		Alts = new float[Enum.GetValues(typeof(AltitudeLayer)).Length];
		AltIncVect = new Vector3(0f, 0.04f, 0f);
		int length = Enum.GetValues(typeof(AltitudeLayer)).Length;
		for (int i = 0; i < length; i++)
		{
			Alts[i] = (float)i * 0.3f;
		}
	}

	public static float AltitudeFor(AltitudeLayer Alt)
	{
		return Alts[(int)Alt];
	}
}
