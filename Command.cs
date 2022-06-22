using System;
using UnityEngine;

public abstract class Command
{
	public const int Size = 50;

	public TooltipDef tipDef;

	public Texture2D icon;

	public Action action;

	public KeyCode hotKey;

	public bool invisible;

	public AudioClip clickSound;

	public bool disabled;

	public string disabledReason;

	protected readonly Texture2D MouseoverTex = Res.LoadTexture("UI/Commands/Mouseover");

	protected readonly Texture2D BGTex = Res.LoadTexture("UI/Commands/CommandBG");

	protected readonly Texture2D MissingIconTex = GenUI.MissingContentTex;

	public virtual AudioClip CurClickSound => clickSound;

	public virtual bool DoButtonGUI(IntVec2 coords)
	{
		GenUI.SetFontSmall();
		Rect rect = ButtonRect(coords);
		GUI.DrawTexture(rect, BGTex);
		Texture2D tex = ((!(icon != null)) ? MissingIconTex : icon);
		UIWidgets.DrawTextureFitted(rect, tex, 1f);
		if (!disabled && rect.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(rect, MouseoverTex);
		}
		Rect position = new Rect(rect.x + 5f, rect.y + rect.height - 21f, 16f, 18f);
		GUI.DrawTexture(position, GenUI.GrayTextBG);
		GUI.Label(position, hotKey.ToString());
		TooltipDef tooltipDef = new TooltipDef(tipDef);
		if (disabled && disabledReason != string.Empty)
		{
			tooltipDef.tipText = tooltipDef.tipText + "\n\nDISABLED: " + disabledReason;
		}
		TooltipHandler.TipRegion(rect, tooltipDef);
		return (Event.current.type == EventType.KeyDown && Event.current.keyCode == hotKey) || UIWidgets.InvisibleButton(rect);
	}

	protected Rect ButtonRect(IntVec2 coords)
	{
		return new Rect(coords.x * 50, coords.z * 50, 50f, 50f);
	}

	public virtual bool ShareClicksFrom(Command other)
	{
		return true;
	}

	public void Disable(string Reason)
	{
		disabled = true;
		disabledReason = Reason;
	}

	public bool Matches(Command other)
	{
		return GetHashCode() == other.GetHashCode();
	}

	public override int GetHashCode()
	{
		int num = 0;
		num = (int)hotKey * 397;
		return num * icon.GetHashCode();
	}

	public override string ToString()
	{
		return "CommandOption:(tiptext=" + tipDef.tipText + ")";
	}
}
