using UnityEngine;

public abstract class UIPanel
{
	public void PanelOnGUI()
	{
		PanelOnGUI(new Rect(0f, 0f, 0f, 0f));
	}

	public abstract void PanelOnGUI(Rect fillRect);

	public virtual void PanelUpdate()
	{
	}

	public virtual void PanelOpened()
	{
	}

	public virtual void PanelClosing()
	{
	}
}
