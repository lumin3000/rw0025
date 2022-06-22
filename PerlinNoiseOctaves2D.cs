using UnityEngine;

public class PerlinNoiseOctaves2D
{
	private PerlinCore core;

	private PerlinNoiseOctaves2DConfig config;

	private double widthDivisor;

	private double heightDivisor;

	public PerlinNoiseOctaves2D(PerlinNoiseOctaves2DConfig config)
	{
		this.config = config;
		core = new PerlinCore(Random.Range(0, 255));
		widthDivisor = 1f / config.FieldSize.x;
		heightDivisor = 1f / config.FieldSize.y;
	}

	public float NoiseAt(int x, int z)
	{
		double num = 0.0;
		num += (core.Noise((double)(4 * x) * widthDivisor, (double)(4 * z) * heightDivisor, -0.5) + 1.0) / 2.0 * (double)config.AmpLow;
		num += (core.Noise((double)(8 * x) * widthDivisor, (double)(8 * z) * heightDivisor, 0.0) + 1.0) / 2.0 * (double)config.AmpMid;
		num += (core.Noise((double)(16 * x) * widthDivisor, (double)(16 * z) * heightDivisor, 0.5) + 1.0) / 2.0 * (double)config.AmpHigh;
		if (num < 0.0)
		{
			num = 0.0;
		}
		if (num > 1.0)
		{
			num = 1.0;
		}
		return (float)num;
	}
}
