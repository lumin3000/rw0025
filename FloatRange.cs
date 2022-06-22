using UnityEngine;

public struct FloatRange
{
	public float min;

	public float max;

	public static FloatRange zero => new FloatRange(0f, 0f);

	public static FloatRange one => new FloatRange(1f, 1f);

	public float Average => (min + max) / 2f;

	public float RandomInRange => Random.Range(min, max);

	public FloatRange(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	public float LerpThroughRange(float lerpPct)
	{
		return (1f - lerpPct) * min + lerpPct * max;
	}
}
