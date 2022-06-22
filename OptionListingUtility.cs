using System.Collections.Generic;
using UnityEngine;

public static class OptionListingUtility
{
	public static void DrawOptionListing(Rect fillRect, List<ListableOption> optList)
	{
		GUI.BeginGroup(fillRect);
		GenUI.SetFontSmall();
		float num = 0f;
		foreach (ListableOption opt in optList)
		{
			opt.DrawOption(new Rect(0f, num, fillRect.width, opt.height));
			num += opt.height + opt.overSpace;
		}
		GUI.EndGroup();
	}

	public static float PaddedYSizeOf(List<ListableOption> optList)
	{
		return 57 * optList.Count - 12;
	}
}
