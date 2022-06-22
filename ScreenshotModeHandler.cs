using UnityEngine;

public class ScreenshotModeHandler
{
	private bool active;

	public bool ShouldFilterCurrentEvent => active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout);

	public void ScreenshotModesOnGUI()
	{
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F12)
		{
			active = !active;
			Event.current.Use();
		}
	}
}
