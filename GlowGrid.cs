using System.Collections.Generic;
using UnityEngine;

public class GlowGrid
{
	private const int GroundGlowOverlitThreshold = 122;

	public Color32[,,] glowGrid;

	private List<CompGlower> allGlowers = new List<CompGlower>();

	private bool glowGridDirty;

	private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

	public GlowGrid()
	{
		glowGrid = new Color32[Find.Map.Size.x, Find.Map.Size.y, Find.Map.Size.z];
	}

	public Color32 GlowAt(IntVec3 loc)
	{
		return glowGrid[loc.x, loc.y, loc.z];
	}

	public PsychGlow PsychGlowAt(IntVec3 loc)
	{
		int num = 0;
		if (!Find.RoofGrid.SquareIsRoofed(loc))
		{
			num += Mathf.RoundToInt(255f * SkyManager.curSkyGlowPercent);
		}
		if (num >= 230)
		{
			return PsychGlow.Overlit;
		}
		Color32 color = glowGrid[loc.x, loc.y, loc.z];
		int num2 = Mathf.RoundToInt((float)(color.r + color.g + color.b) / 3f);
		if (num2 > 122)
		{
			return PsychGlow.Overlit;
		}
		int num3 = num + num2;
		if (num3 < 45)
		{
			return PsychGlow.Dark;
		}
		return PsychGlow.Lit;
	}

	public void RegisterGlower(CompGlower newGlow)
	{
		allGlowers.Add(newGlow);
		MarkGlowGridDirty(newGlow.parent.Position);
		if (!Find.Map.initialized)
		{
			initialGlowerLocs.Add(newGlow.parent.Position);
		}
	}

	public void DeRegisterGlower(CompGlower oldGlow)
	{
		allGlowers.Remove(oldGlow);
		MarkGlowGridDirty(oldGlow.parent.Position);
	}

	public void MarkGlowGridDirty(IntVec3 loc)
	{
		glowGridDirty = true;
		Find.MapDrawer.MapChanged(loc, MapChangeType.GroundGlow);
	}

	public void GlowGridUpdate_First()
	{
		if (glowGridDirty)
		{
			RecalculateAllGlow();
			glowGridDirty = false;
		}
	}

	private void RecalculateAllGlow()
	{
		if (!Find.Map.initialized)
		{
			return;
		}
		if (initialGlowerLocs != null)
		{
			foreach (IntVec3 initialGlowerLoc in initialGlowerLocs)
			{
				MarkGlowGridDirty(initialGlowerLoc);
			}
			initialGlowerLocs = null;
		}
		for (int i = 0; i < Find.Map.Size.x; i++)
		{
			for (int j = 0; j < Find.Map.Size.y; j++)
			{
				for (int k = 0; k < Find.Map.Size.z; k++)
				{
					glowGrid[i, j, k] = new Color32(0, 0, 0, 0);
				}
			}
		}
		foreach (CompGlower allGlower in allGlowers)
		{
			CompPowerTrader comp = allGlower.parent.GetComp<CompPowerTrader>();
			if (comp == null || comp.PowerOn)
			{
				GlowFlooder.AddFloodGlowFor(allGlower, glowGrid);
			}
		}
	}
}
