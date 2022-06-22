using System.Collections.Generic;
using UnityEngine;

public static class FadedMaterialPool
{
	private const int NumFadeSteps = 100;

	private static Dictionary<int, Material> fadedMatsByHash = new Dictionary<int, Material>();

	public static Material FadedVersionOf(Material sourceMat, float alpha)
	{
		if (Debug.isDebugBuild && !sourceMat.HasProperty("_Color"))
		{
			Debug.LogWarning(string.Concat("Cannot fade material ", sourceMat, " since it lacks a color property."));
			return sourceMat;
		}
		int num = IndexFromAlpha(alpha);
		switch (num)
		{
		case 0:
			return MatsSimple.ClearMaterial;
		case 99:
			return sourceMat;
		default:
		{
			int key = HashOf(sourceMat, num);
			if (fadedMatsByHash.TryGetValue(key, out var value))
			{
				return value;
			}
			value = new Material(sourceMat);
			value.color = new Color(1f, 1f, 1f, (float)IndexFromAlpha(alpha) / 100f);
			fadedMatsByHash.Add(key, value);
			return value;
		}
		}
	}

	private static int IndexFromAlpha(float alpha)
	{
		int num = Mathf.FloorToInt(alpha * 100f);
		if (num == 100)
		{
			num = 99;
		}
		return num;
	}

	private static int HashOf(Material mat, int alphaIndex)
	{
		int hashCode = mat.GetHashCode();
		return hashCode * alphaIndex;
	}
}
