using System.Collections.Generic;
using UnityEngine;

public class DialogBox_PawnChooser : DialogBox
{
	public delegate void PawnSelectedByPawnChooser(Pawn SelectedPawn);

	protected string Title;

	private PawnSelectedByPawnChooser CallbackMethod;

	public DialogBox_PawnChooser(string newTitle, PawnSelectedByPawnChooser newCallback)
	{
		Title = newTitle;
		CallbackMethod = newCallback;
		SetWinCentered(900f, 600f);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		GenUI.SetFontMedium();
		GUI.Label(winRect.GetInnerRect(20f), Title);
		List<Pawn> colonists = Find.PawnManager.Colonists;
		Vector2 vector = new Vector2(winRect.x + 20f, winRect.y + 60f);
		GenUI.SetFontSmall();
		foreach (Pawn item in colonists)
		{
			if (GUI.Button(new Rect(vector.x, vector.y, 260f, 28f), item.Label))
			{
				CallbackMethod(item);
				Find.Dialogs.PopBox();
			}
			vector.y += 28f;
		}
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
	}
}
