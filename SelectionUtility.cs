using System;
using System.Collections.Generic;
using UnityEngine;

public static class SelectionUtility
{
	public static IEnumerable<Thing> MultiSelectableThingsInRect(Rect rect)
	{
		Vector2 screenTL = new Vector2(rect.x, (float)Screen.height - rect.y);
		Vector2 screenBR = new Vector2(rect.x + rect.width, (float)Screen.height - (rect.y + rect.height));
		Vector3 worldTL = Gen.ScreenToWorldPoint(screenTL);
		Vector3 worldBR = Gen.ScreenToWorldPoint(screenBR);
		int minX = (int)Math.Floor(worldTL.x);
		int maxX = (int)Math.Ceiling(worldBR.x);
		int minZ = (int)Math.Floor(worldBR.z);
		int maxZ = (int)Math.Ceiling(worldTL.z);
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minZ; j <= maxZ; j++)
			{
				IntVec3 loc = new IntVec3(i, 0, j);
				foreach (Thing t in Find.Grids.ThingsAt(loc))
				{
					if (t.def.selectable && !t.def.neverMultiSelect)
					{
						yield return t;
					}
				}
			}
		}
	}
}
