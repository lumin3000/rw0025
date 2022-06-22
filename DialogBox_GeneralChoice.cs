using UnityEngine;

public class DialogBox_GeneralChoice : DialogBox
{
	private DialogBoxConfig config;

	public DialogBox_GeneralChoice(DialogBoxConfig config)
	{
		this.config = config;
		clearDialogStack = false;
		SetWinCentered(600f, 400f);
		if (config.buttonAAction == null)
		{
			config.buttonAText = "OK";
		}
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(20f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontSmall();
		GUI.Label(new Rect(0f, 0f, innerRect.width, innerRect.height), config.text);
		if (config.buttonAText != string.Empty && UIWidgets.TextButton(new Rect(0f, innerRect.height - 35f, innerRect.width / 2f - 20f, 35f), config.buttonAText))
		{
			if (config.buttonAAction != null)
			{
				config.buttonAAction();
			}
			Find.UIRoot.dialogs.PopBox();
		}
		if (config.buttonBText != string.Empty && UIWidgets.TextButton(new Rect(innerRect.width / 2f + 20f, innerRect.height - 35f, innerRect.width / 2f - 20f, 35f), config.buttonBText))
		{
			if (config.buttonBAction != null)
			{
				config.buttonBAction();
			}
			Find.UIRoot.dialogs.PopBox();
		}
		GUI.EndGroup();
		GenUI.AbsorbAllInput();
	}
}
