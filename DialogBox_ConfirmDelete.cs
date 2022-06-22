using System.IO;
using UnityEngine;

public class DialogBox_ConfirmDelete : DialogBox
{
	private FileInfo Map;

	public DialogBox_ConfirmDelete(FileInfo Map)
	{
		this.Map = Map;
		clearDialogStack = false;
		SetWinCentered(500f, 200f);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(20f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontSmall();
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect(0f, 20f, innerRect.width, innerRect.height), "Really delete " + Map.Name + "?");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.color = Color.red;
		if (UIWidgets.TextButton(new Rect(0f, innerRect.height - 35f, innerRect.width / 2f - 20f, 35f), "Confirm DELETE"))
		{
			Map.Delete();
			Find.UIRoot.dialogs.PopBox();
		}
		GUI.color = Color.white;
		if (UIWidgets.TextButton(new Rect(innerRect.width / 2f + 20f, innerRect.height - 35f, innerRect.width / 2f - 20f, 35f), "Go back"))
		{
			Find.UIRoot.dialogs.PopBox();
		}
		GUI.EndGroup();
		GenUI.AbsorbAllInput();
	}
}
