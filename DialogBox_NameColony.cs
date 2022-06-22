using UnityEngine;

public class DialogBox_NameColony : DialogBox
{
	private Pawn suggestingPawn;

	private string curName = "Crashville";

	public DialogBox_NameColony()
	{
		clearDialogStack = false;
		SetWinCentered(500f, 200f);
		suggestingPawn = Find.PawnManager.Colonists.RandomElement();
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(26f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontSmall();
		GUI.Label(new Rect(0f, 0f, innerRect.width, innerRect.height), "Everyone knows that you don't want to stay on this rock for long. But " + suggestingPawn.characterName + " is suggesting that you give the colony a name anyway.\n\nWhat should it be called?");
		GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
		curName = GUI.TextField(new Rect(0f, innerRect.height - 35f, innerRect.width / 2f - 20f, 35f), curName);
		if (UIWidgets.TextButton(new Rect(innerRect.width / 2f + 20f, innerRect.height - 35f, innerRect.width / 2f - 20f, 35f), "OK"))
		{
			if (IsValidColonyName(curName))
			{
				Find.Map.info.fileName = curName;
				Find.ColonyInfo.ColonyName = curName;
				Find.UIRoot.dialogs.PopBox();
				UI_Messages.Message("The colony is now known as " + curName + ".", UIMessageSound.Benefit);
			}
			else
			{
				UI_Messages.Message("That colony name isn't valid.", UIMessageSound.Reject);
			}
		}
		GUI.EndGroup();
		GenUI.AbsorbAllInput();
	}

	private bool IsValidColonyName(string s)
	{
		if (s.Length == 0)
		{
			return false;
		}
		if (!GenText.IsValidFilename(s))
		{
			return false;
		}
		return true;
	}
}
