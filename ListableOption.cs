using System;
using UnityEngine;

public class ListableOption
{
	public string label;

	public Action action;

	public float overSpace = 12f;

	public float height = 45f;

	public ListableOption(string label, Action action)
	{
		this.label = label;
		this.action = action;
	}

	public virtual void DrawOption(Rect rect)
	{
		if (UIWidgets.TextButton(rect, label, doMouseoverSound: true))
		{
			action();
		}
	}
}
