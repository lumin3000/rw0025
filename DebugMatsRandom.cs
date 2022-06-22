using UnityEngine;

public static class DebugMatsRandom
{
	public const int MaterialCount = 100;

	private const float Opacity = 0.25f;

	private static readonly Material[] mats;

	static DebugMatsRandom()
	{
		mats = new Material[100];
		for (int i = 0; i < 100; i++)
		{
			mats[i] = GenRender.SolidColorMaterial(new Color(Random.value, Random.value, Random.value, 0.25f));
		}
	}

	public static Material Mat(int ind)
	{
		return mats[ind];
	}
}
