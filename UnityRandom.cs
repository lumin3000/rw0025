using System.Collections.Generic;
using NPack;
using UnityEngine;
using URandom;

public class UnityRandom
{
	public enum Normalization
	{
		STDNORMAL,
		POWERLAW
	}

	private MersenneTwister twister;

	public UnityRandom()
	{
		twister = new MersenneTwister();
	}

	public UnityRandom(int seed)
	{
		twister = new MersenneTwister(seed);
	}

	public float Value()
	{
		return twister.NextSingle(includeOne: true);
	}

	public float Value(Normalization n, float t)
	{
		return n switch
		{
			Normalization.STDNORMAL => (float)NormalDistribution.Normalize(twister.NextSingle(includeOne: true), t), 
			Normalization.POWERLAW => (float)PowerLaw.Normalize(twister.NextSingle(includeOne: true), t, 0f, 1f), 
			_ => twister.NextSingle(includeOne: true), 
		};
	}

	public float Range(int minValue, int maxValue)
	{
		return twister.Next(minValue, maxValue);
	}

	public int RangeInt(int minValue, int maxValue)
	{
		return twister.Next(minValue, maxValue - 1);
	}

	public float Range(float min, float max)
	{
		return min + twister.NextSingle(includeOne: true) * (max - min);
	}

	public float Range(int minValue, int maxValue, Normalization n, float t)
	{
		return n switch
		{
			Normalization.STDNORMAL => SpecialFunctions.ScaleFloatToRange((float)NormalDistribution.Normalize(twister.NextSingle(includeOne: true), t), minValue, maxValue, 0f, 1f), 
			Normalization.POWERLAW => (float)PowerLaw.Normalize(twister.NextSingle(includeOne: true), t, minValue, maxValue), 
			_ => twister.Next(minValue, maxValue), 
		};
	}

	public float Possion(float lambda)
	{
		return PoissonDistribution.Normalize(ref twister, lambda);
	}

	public float Exponential(float lambda)
	{
		return ExponentialDistribution.Normalize(twister.NextSingle(includeOne: false), lambda);
	}

	public float Gamma(float order)
	{
		return GammaDistribution.Normalize(ref twister, (int)order);
	}

	public Color Rainbow()
	{
		return WaveToRgb.LinearToRgb(twister.NextSingle(includeOne: true));
	}

	public Color Rainbow(Normalization n, float t)
	{
		return n switch
		{
			Normalization.STDNORMAL => WaveToRgb.LinearToRgb((float)NormalDistribution.Normalize(twister.NextSingle(includeOne: true), t)), 
			Normalization.POWERLAW => WaveToRgb.LinearToRgb((float)PowerLaw.Normalize(twister.NextSingle(includeOne: true), t, 0f, 1f)), 
			_ => WaveToRgb.LinearToRgb(twister.NextSingle(includeOne: true)), 
		};
	}

	public DiceRoll RollDice(int size, DiceRoll.DiceType type)
	{
		return new DiceRoll(size, type, ref twister);
	}

	public ShuffleBagCollection<T> ShuffleBag<T>(T[] values)
	{
		ShuffleBagCollection<T> shuffleBagCollection = new ShuffleBagCollection<T>(twister);
		foreach (T item in values)
		{
			shuffleBagCollection.Add(item);
		}
		return shuffleBagCollection;
	}

	public ShuffleBagCollection<T> ShuffleBag<T>(Dictionary<T, int> dict)
	{
		ShuffleBagCollection<T> shuffleBagCollection = new ShuffleBagCollection<T>(twister);
		foreach (KeyValuePair<T, int> item in dict)
		{
			int value = item.Value;
			T key = item.Key;
			shuffleBagCollection.Add(key, value);
		}
		return shuffleBagCollection;
	}

	public Vector2 PointInASquare()
	{
		return RandomSquare.Area(ref twister);
	}

	public Vector2 PointInASquare(Normalization n, float t)
	{
		return RandomSquare.Area(ref twister, n, t);
	}

	public Vector2 PointInACircle()
	{
		return RandomDisk.Circle(ref twister);
	}

	public Vector2 PointInACircle(Normalization n, float t)
	{
		return RandomDisk.Circle(ref twister, n, t);
	}

	public Vector2 PointInADisk()
	{
		return RandomDisk.Disk(ref twister);
	}

	public Vector2 PointInADisk(Normalization n, float t)
	{
		return RandomDisk.Disk(ref twister, n, t);
	}

	public Vector3 PointInACube()
	{
		return RandomCube.Volume(ref twister);
	}

	public Vector3 PointInACube(Normalization n, float t)
	{
		return RandomCube.Volume(ref twister, n, t);
	}

	public Vector3 PointOnACube()
	{
		return RandomCube.Surface(ref twister);
	}

	public Vector3 PointOnACube(Normalization n, float t)
	{
		return RandomCube.Surface(ref twister, n, t);
	}
}
