using System;
using System.Text;
using UnityEngine;

public class UI_DebugModeReadout
{
	private const float TextWidth = 900f;

	private const float ColumnIntervalX = 300f;

	private const int NumLinesPerColumn = 55;

	private readonly Vector2 ReadoutTL = new Vector2(100f, 100f);

	public void SquareContentsOnGUI()
	{
		if (Game.InEditMode)
		{
			GenUI.SetFontMedium();
			Vector2 readoutTL = ReadoutTL;
			GUI.Label(new Rect(readoutTL.x, 5f, 200f, 200f), "EDIT MODE");
		}
		GenUI.SetFontTiny();
		IntVec3 intVec = Gen.MouseWorldSquare();
		if (!intVec.InBounds())
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (Game.InEditMode)
		{
			stringBuilder.AppendLine(intVec.ToString());
			Thing thing = Find.Grids.BlockerAt(intVec);
			if (thing == null)
			{
				stringBuilder.AppendLine("Blocker: null");
			}
			else
			{
				stringBuilder.AppendLine("Blocker: " + thing.ToString());
			}
		}
		if (DebugSettings.drawReachability)
		{
			stringBuilder.AppendLine("Reachability zone: " + Find.ReachabilityRegions.RegionIndexAt(intVec));
		}
		if (DebugSettings.drawRooms)
		{
			Room roomAt = Find.Grids.GetRoomAt(intVec);
			if (roomAt != null)
			{
				stringBuilder.AppendLine("Room: " + Find.Grids.GetRoomAt(intVec).ToString());
			}
			else
			{
				stringBuilder.AppendLine("Room: null");
			}
		}
		if (DebugSettings.reportGlow)
		{
			stringBuilder.AppendLine("Psych glow: " + Find.GlowGrid.PsychGlowAt(intVec));
			stringBuilder.AppendLine("GridGlow: " + Find.GlowGrid.GlowAt(intVec));
			stringBuilder.AppendLine("GlowReport:\n" + ((MapSectionLayer_LightingOverlay)Find.Map.mapDrawer.SectionAt(intVec).GetLayer(typeof(MapSectionLayer_LightingOverlay))).GlowReportAt(intVec));
			stringBuilder.AppendLine("curSkyGlowPercent: " + SkyManager.curSkyGlowPercent);
		}
		if (DebugSettings.reportPathCosts)
		{
			stringBuilder.AppendLine("Perceived path cost: " + Find.PathGrid.PerceivedPathCostAt(intVec));
			stringBuilder.AppendLine("Real path cost: " + PathGrid.CalculatedCostAt(intVec, perceived: false));
		}
		if (DebugSettings.reportFertility)
		{
			stringBuilder.AppendLine("\nFertility: " + Find.FertilityGrid.FertilityAt(intVec).ToString("##0.00"));
		}
		if (DebugSettings.reportLinkFlags)
		{
			stringBuilder.AppendLine("\nLinkFlags: ");
			foreach (object value in Enum.GetValues(typeof(LinkFlags)))
			{
				if (((uint)LinkGrid.LinkFlagsAt(intVec) & (uint)(int)value) != 0)
				{
					stringBuilder.Append(" " + value);
				}
			}
		}
		if (DebugSettings.drawReportPower)
		{
			foreach (Thing item in Find.Grids.ThingsAt(intVec))
			{
				ThingWithComponents thingWithComponents = item as ThingWithComponents;
				if (thingWithComponents != null && thingWithComponents.GetComp<CompPowerTrader>() != null)
				{
					stringBuilder.AppendLine(" " + thingWithComponents.GetComp<CompPowerTrader>().DebugString);
				}
			}
			PowerNet powerNet = PowerNetGrid.TransmittedPowerNetAt(intVec);
			if (powerNet != null)
			{
				stringBuilder.AppendLine(string.Empty + powerNet.ToString());
			}
			else
			{
				stringBuilder.AppendLine("(no PowerNet here)");
			}
		}
		if (DebugSettings.writeStoryteller)
		{
			stringBuilder.AppendLine(Find.Storyteller.DebugReadout);
		}
		if (Game.InEditMode)
		{
			foreach (Designation item2 in Find.DesignationManager.AllDesignationsAt(intVec))
			{
				stringBuilder.AppendLine(item2.ToString());
			}
			stringBuilder.AppendLine();
			foreach (Thing item3 in Find.Grids.ThingsAt(intVec))
			{
				if (Game.editMode == EditMode.Simple)
				{
					stringBuilder.AppendLine(item3.ToString());
				}
				if (Game.editMode == EditMode.Full)
				{
					stringBuilder.AppendLine(ScribeWrite.ScribeString(item3));
				}
				stringBuilder.AppendLine();
			}
		}
		DrawDebugString(stringBuilder.ToString());
	}

	private void DrawDebugString(string fullString)
	{
		string[] array = fullString.Split('\n');
		int num = 0;
		Vector2 readoutTL = ReadoutTL;
		float y = readoutTL.y;
		for (int i = 0; i < 10; i++)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < 55; j++)
			{
				if (num >= array.Length)
				{
					break;
				}
				stringBuilder.AppendLine(array[num]);
				num++;
			}
			Vector2 readoutTL2 = ReadoutTL;
			Rect position = new Rect(readoutTL2.x + 300f * (float)i, y, 900f, 9999f);
			GUI.Label(position, stringBuilder.ToString());
			if (num >= array.Length)
			{
				break;
			}
		}
	}

	public void DebugReadoutUpdate()
	{
		if (Game.InEditMode)
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
