using System;
using UnityEngine;

public class Command_Toggle : Command
{
	public Func<bool> isActive;

	public override AudioClip CurClickSound
	{
		get
		{
			if (isActive())
			{
				return UISounds.CheckboxTurnedOff;
			}
			return UISounds.CheckboxTurnedOn;
		}
	}

	public override bool DoButtonGUI(IntVec2 coords)
	{
		bool result = base.DoButtonGUI(coords);
		Rect rect = ButtonRect(coords);
		Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
		Texture2D image = ((!isActive()) ? UIWidgets.CheckboxOffTex : UIWidgets.CheckboxOnTex);
		GUI.DrawTexture(position, image);
		return result;
	}

	public override bool ShareClicksFrom(Command other)
	{
		Command_Toggle command_Toggle = (Command_Toggle)other;
		return command_Toggle.isActive() == isActive();
	}
}
