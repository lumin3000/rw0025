using System.Collections.Generic;
using UnityEngine;

public static class WindManager
{
	private const float AmplitudeMultiplier = 0.35f;

	private const float FrequencyMultiplier = 0.025f;

	private static List<Material> plantMaterials = new List<Material>();

	public static void UpdateWindVector()
	{
		float curWindIntensity = Find.WeatherManager.CurWindIntensity;
		float num = Find.TickManager.tickCount;
		float num2 = Mathf.Lerp(0.025f, curWindIntensity * 0.025f, 0.5f);
		Vector4 vector = new Vector4(SimplexNoise.Generate(num * num2), 0f, SimplexNoise.Generate(num * num2 * 1.01f), num);
		vector.z *= 0.5f;
		vector *= curWindIntensity * 0.35f;
		foreach (Material plantMaterial in plantMaterials)
		{
			plantMaterial.SetVector("_WindVector", vector);
		}
	}

	public static void Notify_PlantMaterialCreated(Material newMat)
	{
		plantMaterials.Add(newMat);
	}
}
