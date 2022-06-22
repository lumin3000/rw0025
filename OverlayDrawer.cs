using System;
using System.Collections.Generic;
using UnityEngine;

public static class OverlayDrawer
{
	private const int AltitudeIndex_Forbidden = 4;

	private const int AltitudeIndex_BurningWick = 5;

	private const float PulseFrequency = 4f;

	private const float PulseAmplitude = 0.7f;

	private const float StackOffsetMultipiler = 0.25f;

	private static Dictionary<Thing, OverlayTypes> overlaysToDraw;

	private static Vector3 curOffset;

	private static readonly Material NeedsPowerMat;

	private static readonly Material PowerOffMat;

	private static readonly Material NeedsO2Mat;

	private static readonly Material DamagedMat;

	private static readonly Mesh DamagedMeshSmall;

	private static readonly Mesh DamagedMeshMedium;

	private static readonly Mesh DamagedMeshLarge;

	private static readonly Material ForbiddenMat;

	private static readonly Material WickMaterialA;

	private static readonly Material WickMaterialB;

	private static float SingleSquareForbiddenOffset;

	private static readonly float BaseAlt;

	static OverlayDrawer()
	{
		overlaysToDraw = new Dictionary<Thing, OverlayTypes>();
		NeedsPowerMat = MaterialPool.MatFrom("UI/Overlays/NeedsPower", MatBases.MetaOverlay);
		PowerOffMat = MaterialPool.MatFrom("UI/Overlays/PowerOff", MatBases.MetaOverlay);
		NeedsO2Mat = MaterialPool.MatFrom("UI/Overlays/NeedsO2", MatBases.MetaOverlay);
		DamagedMat = MaterialPool.MatFrom("UI/Overlays/Damaged", MatBases.MetaOverlay);
		DamagedMeshSmall = MeshPool.plane03;
		DamagedMeshMedium = MeshPool.plane05;
		DamagedMeshLarge = MeshPool.plane08;
		ForbiddenMat = MaterialPool.MatFrom("Icons/Special/ForbiddenOverlay", MatBases.MetaOverlay);
		WickMaterialA = MaterialPool.MatFrom("Icons/Special/BurningWickA", MatBases.MetaOverlay);
		WickMaterialB = MaterialPool.MatFrom("Icons/Special/BurningWickB", MatBases.MetaOverlay);
		SingleSquareForbiddenOffset = 0.35f;
		BaseAlt = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
	}

	public static void DrawOverlay(Thing t, OverlayTypes overlayType)
	{
		if (overlaysToDraw.ContainsKey(t))
		{
			Dictionary<Thing, OverlayTypes> dictionary;
			Dictionary<Thing, OverlayTypes> dictionary2 = (dictionary = overlaysToDraw);
			Thing key;
			Thing key2 = (key = t);
			OverlayTypes overlayTypes = dictionary[key];
			dictionary2[key2] = overlayTypes | overlayType;
		}
		else
		{
			overlaysToDraw.Add(t, overlayType);
		}
	}

	public static void DrawAllOverlays()
	{
		foreach (KeyValuePair<Thing, OverlayTypes> item in overlaysToDraw)
		{
			curOffset = Vector3.zero;
			Thing key = item.Key;
			OverlayTypes value = item.Value;
			if ((value & OverlayTypes.BurningWick) != 0)
			{
				RenderBurningWick(key);
			}
			else
			{
				OverlayTypes overlayTypes = OverlayTypes.NeedsO2 | OverlayTypes.NeedsPower | OverlayTypes.PowerOff | OverlayTypes.Damaged;
				int bitCountOf = Gen.GetBitCountOf((long)(value & overlayTypes));
				float num = StackOffsetFor(item.Key);
				switch (bitCountOf)
				{
				case 1:
					curOffset = Vector3.zero;
					break;
				case 2:
					curOffset = new Vector3(-0.5f * num, 0f, 0f);
					break;
				case 3:
					curOffset = new Vector3(-1.5f * num, 0f, 0f);
					break;
				}
				if ((value & OverlayTypes.NeedsO2) != 0)
				{
					RenderNeedsO2Overlay(key);
				}
				if ((value & OverlayTypes.NeedsPower) != 0)
				{
					RenderNeedsPowerOverlay(key);
				}
				if ((value & OverlayTypes.PowerOff) != 0)
				{
					RenderPowerOffOverlay(key);
				}
				if ((value & OverlayTypes.Damaged) != 0)
				{
					RenderDamagedOverlay(key);
				}
			}
			if ((value & OverlayTypes.Forbidden) != 0)
			{
				RenderForbiddenOverlay(key);
			}
		}
		overlaysToDraw.Clear();
	}

	private static float StackOffsetFor(Thing t)
	{
		return (float)t.RotatedSize.x * 0.25f;
	}

	private static void RenderDamagedOverlay(Thing t)
	{
		float num = (float)t.health / (float)t.def.maxHealth;
		RenderPulsingOverlay(mesh: (num < 0.33f) ? DamagedMeshLarge : ((!(num < 0.66f)) ? DamagedMeshSmall : DamagedMeshMedium), thing: t, mat: DamagedMat, altInd: 0);
	}

	private static void RenderNeedsO2Overlay(Thing t)
	{
		RenderPulsingOverlay(t, NeedsO2Mat, 1);
	}

	private static void RenderNeedsPowerOverlay(Thing t)
	{
		RenderPulsingOverlay(t, NeedsPowerMat, 2);
	}

	private static void RenderPowerOffOverlay(Thing t)
	{
		RenderPulsingOverlay(t, PowerOffMat, 3);
	}

	private static void RenderPulsingOverlay(Thing thing, Material mat, int altInd)
	{
		Mesh plane = MeshPool.plane08;
		RenderPulsingOverlay(thing, mat, altInd, plane);
	}

	private static void RenderPulsingOverlay(Thing thing, Material mat, int altInd, Mesh mesh)
	{
		float num = (Find.RealTime.timeUnpaused + 397f * (float)thing.Position.x) * 4f;
		float num2 = ((float)Math.Sin(num) + 1f) * 0.5f;
		num2 = 0.3f + num2 * 0.7f;
		Material material = FadedMaterialPool.FadedVersionOf(mat, num2);
		Vector3 position = thing.TrueCenter();
		position.y = BaseAlt + 0.04f * (float)altInd;
		position += curOffset;
		curOffset.x += StackOffsetFor(thing);
		Graphics.DrawMesh(mesh, position, Quaternion.identity, material, 0);
	}

	private static void RenderForbiddenOverlay(Thing t)
	{
		Vector3 drawPos = t.DrawPos;
		if (t.RotatedSize.z == 1)
		{
			drawPos.z -= SingleSquareForbiddenOffset;
		}
		else
		{
			drawPos.z -= (float)t.RotatedSize.z * 0.3f;
		}
		drawPos.y = BaseAlt + 0.16f;
		Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, ForbiddenMat, 0);
	}

	private static void RenderBurningWick(Thing parent)
	{
		Material material = ((!(UnityEngine.Random.value < 0.5f)) ? WickMaterialB : WickMaterialA);
		Vector3 drawPos = parent.DrawPos;
		drawPos.y = BaseAlt + 0.19999999f;
		Graphics.DrawMesh(MeshPool.plane20, drawPos, Quaternion.identity, material, 0);
	}

	public static void DrawOverlayBar(Thing t, float percentFull, Texture2D fillTex)
	{
		Vector2 vector = Find.CameraMap.InvertedWorldToScreenPoint(t.DrawPos);
		Rect rect = new Rect(vector.x - 15f, vector.y, 30f, 4f);
		UIWidgets.FillableBar(rect, percentFull, fillTex);
	}
}
