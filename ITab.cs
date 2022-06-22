using UnityEngine;

public abstract class ITab
{
	public bool IsOpen;

	public string Label;

	protected Vector2 Size;

	public virtual bool IsVisible => true;

	protected Thing SelThing => Find.Selector.SingleSelectedThing;

	protected Pawn SelPawn => SelThing as Pawn;

	public void DoGUI()
	{
		float num = Screen.height - 35;
		Vector2 paneSize = UI_InspectPane.PaneSize;
		float top = num - paneSize.y - 30f - Size.y;
		Rect rect = new Rect(0f, top, Size.x, Size.y);
		UIWidgets.DrawWindow(rect);
		if (UIWidgets.CloseButtonFor(rect))
		{
			Find.UIMapRoot.modeControls.tabInspect.inspector.CloseOpenTab();
		}
		GUI.BeginGroup(rect);
		FillTab();
		GUI.EndGroup();
		GenUI.AbsorbClicksInRect(rect);
	}

	protected abstract void FillTab();

	public virtual void Opening()
	{
	}

	public virtual void TabTick()
	{
	}
}
