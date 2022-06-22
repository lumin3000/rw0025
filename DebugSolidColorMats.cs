using System.Collections.Generic;
using UnityEngine;

public static class DebugSolidColorMats
{
	private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();

	public static Material MaterialOf(Color col)
	{
		if (colorMatDict.TryGetValue(col, out var value))
		{
			return value;
		}
		value = GenRender.SolidColorMaterial(col);
		colorMatDict.Add(col, value);
		return value;
	}
}
