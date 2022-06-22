using System;
using System.Collections.Generic;
using UnityEngine;

public static class LongEventHandler
{
	private class QueuedLongEvent
	{
		public Action eventAction;

		public string eventText = string.Empty;

		public int framesUntilEvent = -1;
	}

	private static Queue<QueuedLongEvent> eventQueue = new Queue<QueuedLongEvent>();

	private static QueuedLongEvent displayingEvent = null;

	private static readonly Vector2 GUIRectSize = new Vector2(240f, 75f);

	public static void QueueLongEvent(Action newEventAction, string newEventText)
	{
		QueuedLongEvent queuedLongEvent = new QueuedLongEvent();
		queuedLongEvent.eventAction = newEventAction;
		queuedLongEvent.eventText = newEventText;
		queuedLongEvent.framesUntilEvent = 2;
		eventQueue.Enqueue(queuedLongEvent);
	}

	public static void LongEventsOnGUI()
	{
		if (displayingEvent != null)
		{
			if (displayingEvent.framesUntilEvent > -2)
			{
				float num = Screen.width / 2;
				Vector2 gUIRectSize = GUIRectSize;
				float left = num - gUIRectSize.x / 2f;
				float num2 = (float)Screen.height * 0.5f;
				Vector2 gUIRectSize2 = GUIRectSize;
				float top = num2 - gUIRectSize2.y / 2f;
				Vector2 gUIRectSize3 = GUIRectSize;
				float x = gUIRectSize3.x;
				Vector2 gUIRectSize4 = GUIRectSize;
				Rect rect = new Rect(left, top, x, gUIRectSize4.y);
				UIWidgets.DrawWindow(rect);
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GenUI.SetFontSmall();
				GUI.Label(rect, displayingEvent.eventText);
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GenUI.AbsorbAllInput();
			}
			else
			{
				displayingEvent = null;
			}
		}
	}

	public static void LongEventsUpdate()
	{
		if (displayingEvent != null)
		{
			displayingEvent.framesUntilEvent--;
		}
		if (eventQueue.Count != 0)
		{
			QueuedLongEvent queuedLongEvent = eventQueue.Peek();
			queuedLongEvent.framesUntilEvent--;
			if (queuedLongEvent.framesUntilEvent <= 0)
			{
				queuedLongEvent.eventAction();
				displayingEvent = queuedLongEvent;
				eventQueue.Dequeue();
			}
		}
	}
}
