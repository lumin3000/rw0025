using System;

public class TabDef
{
	public string label = "Tab";

	public Action clickedAction;

	public bool selected;

	public TabDef(string label, Action clickedAction, bool selected)
	{
		this.label = label;
		this.clickedAction = clickedAction;
		this.selected = selected;
	}
}
