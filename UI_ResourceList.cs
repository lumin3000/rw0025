using System;
using UnityEngine;

public class UI_ResourceList
{
	private const float Pad = 10f;

	public void ResourceListOnGUI()
	{
		Rect position = new Rect(8f, 8f, 90f, 270f);
		GUI.BeginGroup(position);
		float num = 0f;
		foreach (int value in Enum.GetValues(typeof(EntityType)))
		{
			if (value != 0 && (Find.ResourceManager.TotalAmountOf((EntityType)value) > 0 || value == 14))
			{
				TradeUI.DrawResource((EntityType)value, num);
				num += 28f;
			}
		}
		GUI.EndGroup();
	}
}
