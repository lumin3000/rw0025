using System.Collections.Generic;
using UnityEngine;

public static class TooltipHandler
{
	private static Dictionary<int, TooltipDef> activeTips = new Dictionary<int, TooltipDef>();

	private static float TooltipDelay = 0.45f;

	public static void ClearTooltipsFrom(Rect rect)
	{
		if (Event.current.type != EventType.Repaint || !rect.Contains(Event.current.mousePosition))
		{
			return;
		}
		foreach (KeyValuePair<int, TooltipDef> item in activeTips.DictFullCopy())
		{
			if (item.Value.lastTriggerFrame == Time.frameCount)
			{
				activeTips.Remove(item.Key);
			}
		}
	}

	public static void TipRegion(Rect rect, TooltipDef tip)
	{
		if (Event.current.type == EventType.Repaint && rect.Contains(Event.current.mousePosition))
		{
			if (DebugSettings.showTooltipEdges)
			{
				GenUI.DrawBox(rect, 1);
			}
			if (!activeTips.ContainsKey(tip.uniqueId))
			{
				activeTips.Add(tip.uniqueId, tip);
				activeTips[tip.uniqueId].firstTriggerTime = Time.realtimeSinceStartup;
			}
			activeTips[tip.uniqueId].lastTriggerFrame = Time.frameCount;
			activeTips[tip.uniqueId].tipText = tip.tipText;
		}
	}

	public static void DoTooltipGUI()
	{
		DrawActiveTips();
		if (Event.current.type == EventType.Repaint)
		{
			CleanActiveTooltips();
		}
	}

	private static void DrawActiveTips()
	{
		List<TooltipDef> list = new List<TooltipDef>();
		foreach (KeyValuePair<int, TooltipDef> activeTip in activeTips)
		{
			if ((double)Time.realtimeSinceStartup > activeTip.Value.firstTriggerTime + (double)TooltipDelay)
			{
				list.Add(activeTip.Value);
			}
		}
		list.Sort(CompareTooltipsByPriority);
		float num = 0f;
		foreach (TooltipDef item in list)
		{
			num += item.DrawTooltip(num);
			num += 2f;
		}
	}

	private static void CleanActiveTooltips()
	{
		foreach (KeyValuePair<int, TooltipDef> item in activeTips.DictFullCopy())
		{
			if (item.Value.lastTriggerFrame != Time.frameCount)
			{
				activeTips.Remove(item.Key);
			}
		}
	}

	private static int CompareTooltipsByPriority(TooltipDef A, TooltipDef B)
	{
		if (A.priority == B.priority)
		{
			return 0;
		}
		if (A.priority == TooltipPriority.Pawn)
		{
			return -1;
		}
		if (B.priority == TooltipPriority.Pawn)
		{
			return 1;
		}
		return 0;
	}
}
