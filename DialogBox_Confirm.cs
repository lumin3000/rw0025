using System;
using UnityEngine;

public class DialogBox_Confirm : DialogBox
{
	private string text;

	private Action confirmedAction;

	public DialogBox_Confirm(string text, Action confirmedAction)
	{
		this.text = text;
		this.confirmedAction = confirmedAction;
		clearDialogStack = false;
		SetWinCentered(500f, 300f);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(20f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontSmall();
		GUI.Label(new Rect(0f, 0f, innerRect.width, innerRect.height), text);
		if (UIWidgets.TextButton(new Rect(0f, innerRect.height - 35f, innerRect.width / 2f - 20f, 35f), "Confirm"))
		{
			confirmedAction();
			Find.UIRoot.dialogs.PopBox();
		}
		if (UIWidgets.TextButton(new Rect(innerRect.width / 2f + 20f, innerRect.height - 35f, innerRect.width / 2f - 20f, 35f), "Go back"))
		{
			Find.UIRoot.dialogs.PopBox();
		}
		GUI.EndGroup();
		GenUI.AbsorbAllInput();
	}
}
