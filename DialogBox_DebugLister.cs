using UnityEngine;

public abstract class DialogBox_DebugLister : DialogBox_Debug
{
	public override void DoDialogBoxGUI()
	{
		GenUI.SetFontSmall();
		GUI.skin.button.alignment = TextAnchor.MiddleLeft;
		curOffset = new Vector2(0f, 0f);
		UIWidgets.DrawWindow(winRect);
		GUI.BeginGroup(winRect.GetInnerRect(10f));
		DoList();
		GUI.EndGroup();
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
		GUI.skin.button.alignment = TextAnchor.UpperLeft;
	}

	protected abstract void DoList();
}
