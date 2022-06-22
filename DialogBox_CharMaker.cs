using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogBox_CharMaker : DialogBox
{
	private const float TopAreaHeight = 80f;

	private Pawn curPawn;

	private static readonly Vector2 WinSize = new Vector2(900f, 750f);

	private List<Pawn> Colonists => MapInitParams.colonists;

	public DialogBox_CharMaker()
	{
		curPawn = Colonists[0];
		SetWinCentered(WinSize);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontMedium();
		GUI.Label(new Rect(0f, 0f, 300f, 300f), "Create characters");
		GenUI.SetFontSmall();
		Rect rect = new Rect(0f, 80f, innerRect.width, innerRect.height - 65f - 80f);
		UIWidgets.DrawMenuSection(rect);
		UIWidgets.DrawTabs(rect, Colonists.Select((Pawn c) => new TabDef(c.Label, delegate
		{
			SelectConfig(c);
		}, c == curPawn)));
		Rect innerRect2 = rect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect2);
		Action randomizeCallback = delegate
		{
			int index = Colonists.IndexOf(curPawn);
			Colonists.Remove(curPawn);
			Pawn item = PawnMaker.GeneratePawn("Colonist");
			Colonists.Insert(index, item);
			curPawn = item;
		};
		PawnCardUtility.DrawPawnCard(curPawn, randomizeCallback);
		GUI.EndGroup();
		Action nextAct = delegate
		{
			AcceptanceReport acceptanceReport = CanStart();
			if (acceptanceReport.accepted)
			{
				DoneMakingCharacters();
			}
			else
			{
				UI_Messages.Message(acceptanceReport.reasonText);
			}
		};
		EntryDialogUtility.DoNextBackButtons(winRect, "Start", nextAct, delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_StorytellerChooser());
		});
		GUI.EndGroup();
	}

	private void DoneMakingCharacters()
	{
		MapInitParams.startedFromEntry = true;
		foreach (Pawn colonist in Colonists)
		{
			colonist.story.MakeSkillsFromHistory();
		}
		LongEventHandler.QueueLongEvent(delegate
		{
			Application.LoadLevel("Gameplay");
		}, "Generating world...");
	}

	private AcceptanceReport CanStart()
	{
		foreach (Pawn colonist in Colonists)
		{
			if (colonist.characterName.Length == 0)
			{
				return new AcceptanceReport("Each person needs a name.");
			}
		}
		return AcceptanceReport.WasAccepted;
	}

	private void SelectConfig(Pawn c)
	{
		if (c != curPawn)
		{
			GenSound.PlaySoundOnCamera(UISounds.SubmenuSelect, 0.2f);
			curPawn = c;
		}
	}
}
