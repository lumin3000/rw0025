using System;
using UnityEngine;

public class FloatMenuChoice
{
	private const float LabelLeftMargin = 10f;

	private const float MouseOverTextJump = 4f;

	private string label;

	private Action act;

	public FloatMenuPriority priority = FloatMenuPriority.Medium;

	public bool autoTakeable;

	public bool disabled;

	public static readonly Vector2 Size = new Vector2(300f, 28f);

	private static readonly Texture2D ChoiceBGTexture = Res.LoadTexture("UI/Widgets/FloatMenuChoiceBG");

	private static readonly Texture2D ChoiceBGTextureDisabled = Res.LoadTexture("UI/Widgets/FloatMenuChoiceBGDisabled");

	private static readonly Color MouseoverColor = new Color(1f, 0.92f, 0.6f);

	public FloatMenuChoice(string label, Action act)
	{
		this.label = label;
		this.act = act;
		if (act == null)
		{
			disabled = true;
		}
	}

	public FloatMenuChoice(string label, Action act, FloatMenuPriority priority)
		: this(label, act)
	{
		this.priority = priority;
	}

	public bool ChoiceButton(Vector2 Root)
	{
		float x = Root.x;
		float y = Root.y;
		Vector2 size = Size;
		float x2 = size.x;
		Vector2 size2 = Size;
		Rect rect = new Rect(x, y, x2, size2.y);
		bool flag = !disabled && rect.Contains(Event.current.mousePosition);
		Texture2D image = ChoiceBGTexture;
		if (disabled)
		{
			image = ChoiceBGTextureDisabled;
		}
		if (flag)
		{
			GUI.color = MouseoverColor;
		}
		GUI.DrawTexture(rect, image);
		if (!disabled)
		{
			MouseoverSounds.DoRegion(rect);
		}
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		Rect position = new Rect(rect);
		position.x += 10f;
		if (flag)
		{
			position.x += 4f;
		}
		if (disabled)
		{
			GUI.color = new Color(0.7f, 0.7f, 0.7f);
		}
		position.width = 999f;
		GUI.Label(position, label);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.white;
		if (UIWidgets.InvisibleButton(rect))
		{
			Chosen();
			Event.current.Use();
			return true;
		}
		return false;
	}

	public void Chosen()
	{
		if (!disabled)
		{
			GenSound.PlaySoundOnCamera("Radio/RadioFuzzClose", 0.02f, SoundSlot.RadioFuzz);
			if (act != null)
			{
				act();
			}
			Find.FloatMenu.Close();
		}
		else
		{
			GenSound.PlaySoundOnCamera(UISounds.ClickReject, 0.1f);
		}
	}
}
