using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Alerts
{
	public const float AlertListWidth = 170f;

	private readonly List<Alert> todoItems = new List<Alert>();

	public static readonly Vector2 ItemSize = new Vector2(150f, 26f);

	private IEnumerable<Alert> ActiveItems => todoItems.Where((Alert item) => item.Active);

	public UI_Alerts()
	{
		foreach (Type item in typeof(Alert).AllSubclasses())
		{
			todoItems.Add((Alert)Activator.CreateInstance(item));
		}
	}

	public void AlertListUpdate()
	{
		foreach (Alert activeItem in ActiveItems)
		{
			activeItem.AlertActiveUpdate();
		}
	}

	public void AlertListOnGUI()
	{
		int num = ActiveItems.Count();
		if (num == 0)
		{
			return;
		}
		float left = (float)Screen.width - 170f;
		float num2 = num;
		Vector2 itemSize = ItemSize;
		Rect baseRect = new Rect(left, 0f, 170f, 20f + num2 * itemSize.y);
		Alert alert = null;
		GUI.BeginGroup(baseRect.GetInnerRect(10f));
		AlertPriority alertPriority = AlertPriority.Critical;
		bool flag = false;
		float num3 = 0f;
		AlertPriority prio;
		foreach (int value in Enum.GetValues(typeof(AlertPriority)))
		{
			prio = (AlertPriority)value;
			IEnumerable<Alert> enumerable = ActiveItems.Where((Alert item) => item.FullPriority == prio);
			foreach (Alert item in enumerable)
			{
				if (!flag)
				{
					alertPriority = prio;
					flag = true;
				}
				float top = num3;
				Vector2 itemSize2 = ItemSize;
				float x = itemSize2.x;
				Vector2 itemSize3 = ItemSize;
				Rect bgRect = new Rect(0f, top, x, itemSize3.y);
				if (bgRect.Contains(Event.current.mousePosition))
				{
					alert = item;
				}
				item.DrawAt(bgRect, prio != alertPriority);
				float num4 = num3;
				Vector2 itemSize4 = ItemSize;
				num3 = num4 + itemSize4.y;
			}
		}
		GUI.EndGroup();
		alert?.DrawInfoPane();
	}
}
