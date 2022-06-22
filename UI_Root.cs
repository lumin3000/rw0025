using UnityEngine;

public abstract class UI_Root
{
	public UI_DialogBoxHandler dialogs = new UI_DialogBoxHandler();

	public UI_FeedbackFloaters feedbackFloaters = new UI_FeedbackFloaters();

	protected ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

	public virtual void UIRootOnGUI()
	{
		screenshotMode.ScreenshotModesOnGUI();
		if (Debug.developerConsoleVisible && GUI.Button(new Rect(400f, Screen.height - 200, 150f, 50f), "Close console"))
		{
			Debug.developerConsoleVisible = false;
		}
		if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
		{
			LongEventHandler.LongEventsOnGUI();
			if (!screenshotMode.ShouldFilterCurrentEvent)
			{
				feedbackFloaters.FeedbackOnGUI();
				DragSliderManager.DragSlidersOnGUI();
				TooltipHandler.DoTooltipGUI();
				UI_Messages.MessagesDoGUI();
			}
			dialogs.DialogBoxesOnGUI();
		}
		else
		{
			dialogs.DialogBoxesOnGUI();
			if (!screenshotMode.ShouldFilterCurrentEvent)
			{
				UI_Messages.MessagesDoGUI();
				TooltipHandler.DoTooltipGUI();
				DragSliderManager.DragSlidersOnGUI();
				feedbackFloaters.FeedbackOnGUI();
			}
			LongEventHandler.LongEventsOnGUI();
		}
	}
}
