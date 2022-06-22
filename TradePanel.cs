using UnityEngine;

public abstract class TradePanel : UIPanel_Tab
{
	public override void PanelOnGUI(Rect fillRect)
	{
	}

	public abstract void PanelOnGUITrade(Vector2 fillSize);
}
