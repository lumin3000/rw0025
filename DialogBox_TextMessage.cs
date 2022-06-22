using UnityEngine;

public class DialogBox_TextMessage : DialogBox
{
	protected string Title;

	private string Text;

	public DialogBox_TextMessage(string newTitle, string newText)
	{
		Title = newTitle;
		Text = newText;
		SetWinCentered(600f, 400f);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		int num = 20;
		if (Title != string.Empty)
		{
			GenUI.SetFontMedium();
			GUI.Label(new Rect(winRect.x + 20f, winRect.y + (float)num, winRect.width - 40f, winRect.height - 100f), Title);
			num += 60;
		}
		GenUI.SetFontSmall();
		GUI.Label(new Rect(winRect.x + 20f, winRect.y + (float)num, winRect.width - 40f, winRect.height - 100f), Text);
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
	}
}
