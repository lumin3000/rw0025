using System.Collections.Generic;
using UnityEngine;

public static class UI_Messages
{
	private class UIMessage
	{
		private const float DefaultMessageLifespan = 10f;

		private const float FadeoutDuration = 0.6f;

		public string text;

		private float startingTime;

		public int startingFrame;

		protected float Age => Time.time - startingTime;

		protected float TimeLeft => 10f - Age;

		public bool Expired => TimeLeft <= 0f;

		public UIMessage(string Text)
		{
			text = Text;
			startingFrame = Time.frameCount;
			startingTime = Time.time;
		}

		public void Draw(int YOffset)
		{
			Vector2 vector = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(text));
			if (TimeLeft < 0.6f)
			{
				float a = TimeLeft / 0.6f;
				GUI.color = new Color(1f, 1f, 1f, a);
			}
			Rect position = new Rect(0f, YOffset, vector.x, 27f);
			GUI.Label(position, text);
			GUI.color = Color.white;
		}
	}

	private const int MessagesLeftX = 140;

	private const int MessagesTopY = 16;

	private const int MessageYInterval = 33;

	private static List<UIMessage> MessageList = new List<UIMessage>();

	public static void Update()
	{
		MessageList.RemoveAll((UIMessage message) => message.Expired);
	}

	public static void Message(string MessageText)
	{
		Message(MessageText, UIMessageSound.Standard);
	}

	public static void Message(string MessageText, UIMessageSound sound)
	{
		if (MessageText != string.Empty)
		{
			foreach (UIMessage message in MessageList)
			{
				if (message.text == MessageText && message.startingFrame == Time.frameCount)
				{
					return;
				}
			}
			UIMessage item = new UIMessage(MessageText);
			MessageList.Add(item);
		}
		if (sound != 0)
		{
			AudioClip clip = null;
			if (sound == UIMessageSound.Standard)
			{
				clip = UISounds.MessageAlert;
			}
			if (sound == UIMessageSound.Reject)
			{
				clip = UISounds.ClickReject;
			}
			if (sound == UIMessageSound.Benefit)
			{
				clip = UISounds.LevelUp;
			}
			if (sound == UIMessageSound.Negative)
			{
				clip = UISounds.MessageAlertNegative;
			}
			if (sound == UIMessageSound.SeriousAlert)
			{
				clip = UISounds.MessageSeriousAlert;
			}
			GenSound.PlaySoundOnCamera(clip, 0.2f, SoundSlot.AlertBeep);
		}
	}

	public static void MessagesDoGUI()
	{
		GenUI.SetFontSmall();
		GUI.BeginGroup(new Rect(140f, 16f, 999f, 999f));
		int num = 0;
		List<UIMessage> list = MessageList.ListFullCopy();
		list.Reverse();
		foreach (UIMessage item in list)
		{
			item.Draw(num);
			num += 33;
		}
		GUI.EndGroup();
	}
}
