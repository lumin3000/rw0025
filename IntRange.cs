using UnityEngine;

public struct IntRange
{
	public int min;

	public int max;

	public static IntRange zero => new IntRange(0, 0);

	public static IntRange one => new IntRange(1, 1);

	public float Average => ((float)min + (float)max) / 2f;

	public int RandomInRange => Random.Range(min, max + 1);

	public IntRange(int min, int max)
	{
		this.min = min;
		this.max = max;
	}

	public int Lerped(float lerpFactor)
	{
		return min + Mathf.RoundToInt(lerpFactor * (float)(max - min));
	}
}
