using System;
using UnityEngine;

public class DebugTool
{
	private string label;

	private Action clickAction;

	public DebugTool(string label, Action clickAction)
	{
		this.label = label;
		this.clickAction = clickAction;
	}

	public void DebugToolOnGUI()
	{
		if (Event.current.type == EventType.MouseDown)
		{
			if (Event.current.button == 0)
			{
				clickAction();
			}
			if (Event.current.button == 1)
			{
				DebugTools.curTool = null;
			}
			Event.current.Use();
		}
		Vector2 vector = Event.current.mousePosition + new Vector2(15f, 15f);
		Rect position = new Rect(vector.x, vector.y, 999f, 999f);
		GenUI.SetFontSmall();
		GUI.Label(position, label);
	}
}
