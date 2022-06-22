using System.Collections.Generic;
using UnityEngine;

public class TooltipGiverList
{
	private List<Thing> TooltipGivers = new List<Thing>();

	public void RegisterTooltipGiver(Thing t)
	{
		if (!t.def.hasTooltip)
		{
			Debug.LogWarning(string.Concat("Tried to register non tooltip giver ", t, " to give tooltips."));
		}
		else
		{
			TooltipGivers.Add(t);
		}
	}

	public void DeregisterTooltipGiver(Thing t)
	{
		if (TooltipGivers.Contains(t))
		{
			TooltipGivers.Remove(t);
		}
		else
		{
			Debug.LogWarning("Tried to remove non-registered tooltip giver " + t);
		}
	}

	public void DispenseAllTooltips()
	{
		if (Find.UIMapRoot.dialogs.TopDialog != null || Find.FloatMenu.active)
		{
			return;
		}
		int num = (int)Find.CameraMap.SquareSize();
		foreach (Thing tooltipGiver in TooltipGivers)
		{
			Vector2 vector = Find.CameraMap.InvertedWorldToScreenPoint(tooltipGiver.DrawPos);
			IntVec2 intVec = tooltipGiver.RotatedSize * num;
			Rect rect = new Rect(vector.x - (float)(intVec.x / 2), vector.y - (float)(intVec.z / 2), intVec.x, intVec.z);
			TooltipDef tooltip = tooltipGiver.GetTooltip();
			if (tooltipGiver.def.eType == EntityType.Pawn)
			{
				tooltip.priority = TooltipPriority.Pawn;
			}
			TooltipHandler.TipRegion(rect, tooltip);
		}
	}
}
