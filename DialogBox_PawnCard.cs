using UnityEngine;

public class DialogBox_PawnCard : DialogBox
{
	private Pawn pawn;

	public DialogBox_PawnCard(Pawn pawn)
	{
		this.pawn = pawn;
		clearDialogStack = false;
		Vector2 winCentered = PawnCardUtility.PawnCardSize + new Vector2(17f, 17f) * 2f + new Vector2(0f, 60f);
		SetWinCentered(winCentered);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect);
		PawnCardUtility.DrawPawnCard(pawn);
		GUI.EndGroup();
		DetectShouldClose(doButton: true);
	}
}
