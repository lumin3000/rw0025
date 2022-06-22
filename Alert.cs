using System;
using UnityEngine;

public abstract class Alert
{
	private const float ItemPeekWidth = 30f;

	public const float InfoRectWidth = 330f;

	private const float CriticalPulseFreq = 3f;

	private const float CriticalPulseAmp = 0.6f;

	protected AlertPriority basePriority;

	protected string baseLabel;

	protected string baseExplanation;

	private int lastActiveFrame = -1;

	private static readonly Texture2D BGTexCritical = GenRender.SolidColorTexture(new Color(0.5f, 0f, 0f));

	private static readonly Texture2D BGTexHighlight = GenUI.HighlightTex;

	public bool Active => Report.active;

	public abstract AlertReport Report { get; }

	public virtual AlertPriority FullPriority => basePriority;

	public virtual string FullLabel => baseLabel;

	public virtual string FullExplanation => baseExplanation;

	public void AlertActiveUpdate()
	{
		if (FullPriority == AlertPriority.Critical)
		{
			if (lastActiveFrame < Time.frameCount - 1)
			{
				UI_Messages.Message("Critical alert: " + FullLabel, UIMessageSound.SeriousAlert);
			}
			lastActiveFrame = Time.frameCount;
		}
	}

	public virtual void DrawAt(Rect bgRect, bool minimized)
	{
		Texture2D texture2D = null;
		if (FullPriority == AlertPriority.Critical)
		{
			texture2D = BGTexCritical;
			float num = ((float)Math.Sin(Time.time * 3f) + 1f) * 0.5f;
			num = 0.39999998f + num * 0.6f;
			GUI.color = new Color(num, num, num);
			GUI.DrawTexture(bgRect, texture2D);
			GUI.color = Color.white;
		}
		GUI.BeginGroup(bgRect);
		GenUI.SetFontSmall();
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect(6f, 0f, 999f, bgRect.height), FullLabel);
		GUI.EndGroup();
		if (bgRect.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(bgRect, BGTexHighlight);
		}
		if (Report.culprit != null && UIWidgets.InvisibleButton(bgRect))
		{
			Find.CameraMap.JumpTo(Report.culprit.Position);
		}
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}

	public void DrawInfoPane()
	{
		GenUI.SetFontSmall();
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		string text = FullExplanation;
		if (Report.culprit != null)
		{
			text += "\n\n(Click to jump to problem)";
		}
		float num = GUI.skin.label.CalcHeight(new GUIContent(text), 310f);
		num += 20f;
		Rect rect = new Rect((float)Screen.width - 170f - 330f, 0f, 330f, num);
		UIWidgets.DrawWindow(rect);
		Rect innerRect = rect.GetInnerRect(10f);
		GUI.BeginGroup(innerRect);
		GUI.Label(new Rect(0f, 0f, innerRect.width, innerRect.height), text);
		GUI.EndGroup();
	}
}
