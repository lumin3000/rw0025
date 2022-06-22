using System.Collections.Generic;
using UnityEngine;

public class DamageFlasher
{
	private const int DamagedMatTicksTotal = 16;

	private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

	private int lastDamageTick = -9999;

	private static readonly Color DamagedMatStartingColor = Color.red;

	private int DamageFlashTicksLeft => lastDamageTick + 16 - Find.TickManager.tickCount;

	public DamageFlasher(Pawn pawn)
	{
	}

	public Material GetCurMatBasedOn(Material baseMat)
	{
		if (DamageFlashTicksLeft > 0)
		{
			if (!damagedMats.TryGetValue(baseMat, out var value))
			{
				value = new Material(baseMat);
				damagedMats.Add(baseMat, value);
			}
			float t = (float)DamageFlashTicksLeft / 16f;
			Color color2 = (value.color = Color.Lerp(Color.white, DamagedMatStartingColor, t));
			return value;
		}
		return baseMat;
	}

	public void Notify_DamageApplied(DamageInfo dinfo)
	{
		if (dinfo.type.HarmsHealth())
		{
			lastDamageTick = Find.TickManager.tickCount;
		}
	}
}
