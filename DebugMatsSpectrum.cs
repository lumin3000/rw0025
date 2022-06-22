using System.Collections.Generic;
using UnityEngine;

public static class DebugMatsSpectrum
{
	public const int MaterialCount = 100;

	private const float Opacity = 0.25f;

	private static List<Material> spectrumMats;

	private static readonly Color[] ColorSpectrum;

	static DebugMatsSpectrum()
	{
		spectrumMats = new List<Material>();
		ColorSpectrum = new Color[6]
		{
			new Color(0.5f, 0.3f, 0f, 0.25f),
			new Color(0f, 0f, 1f, 0.25f),
			new Color(0f, 1f, 0f, 0.25f),
			new Color(0.75f, 0f, 0f, 0.25f),
			new Color(1f, 0.6f, 0.18f, 0.25f),
			new Color(1f, 1f, 1f, 0.25f)
		};
		for (int i = 0; i < 100; i++)
		{
			float num = (float)i / 100f;
			num *= (float)(ColorSpectrum.Length - 1);
			int num2 = 0;
			while (num > 1f)
			{
				num -= 1f;
				num2++;
				if (num2 > ColorSpectrum.Length - 1)
				{
					Debug.LogError("Hit spectrum limit on " + i);
					num2--;
					break;
				}
			}
			Color col = Color.Lerp(ColorSpectrum[num2], ColorSpectrum[num2 + 1], num);
			spectrumMats.Add(GenRender.SolidColorMaterial(col, MatBases.MetaOverlay));
		}
	}

	public static Material Mat(int ind)
	{
		if (ind >= 100)
		{
			ind = 99;
		}
		if (ind < 0)
		{
			ind = 0;
		}
		return spectrumMats[ind];
	}
}
