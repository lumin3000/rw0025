using System;
using UnityEngine;

public class ITab_Pawn_Prisoner : ITab
{
	private const float CheckboxInterval = 30f;

	private const float CheckboxMargin = 50f;

	public override bool IsVisible => base.SelPawn.Team == TeamType.Prisoner;

	public ITab_Pawn_Prisoner()
	{
		Size = new Vector2(300f, 400f);
		Label = "Prisoner";
	}

	protected override void FillTab()
	{
		GenUI.SetFontSmall();
		Rect baseRect = new Rect(0f, 0f, Size.x, Size.y);
		Rect innerRect = baseRect.GetInnerRect(10f);
		GUI.BeginGroup(innerRect);
		Rect rect = new Rect(50f, 0f, innerRect.width - 100f, 30f);
		UIWidgets.LabelCheckbox(rect, "Gets food", ref base.SelPawn.prisoner.getsFood);
		rect.y += 30f;
		UIWidgets.LabelCheckbox(rect, "Try to recruit", ref base.SelPawn.prisoner.tryRecruit);
		float num = rect.y + 30f;
		Rect rect2 = new Rect(0f, num, innerRect.width, innerRect.height - num);
		UIWidgets.DrawMenuSection(rect2);
		Rect innerRect2 = rect2.GetInnerRect(10f);
		GUI.BeginGroup(innerRect2);
		Rect screenRect = new Rect(0f, 0f, innerRect2.width, 30f);
		foreach (int value in Enum.GetValues(typeof(PrisonerInteractionMode)))
		{
			if (UIWidgets.LabelRadioButton(screenRect, LabelOf((PrisonerInteractionMode)value), base.SelPawn.prisoner.interactionMode == (PrisonerInteractionMode)value))
			{
				base.SelPawn.prisoner.interactionMode = (PrisonerInteractionMode)value;
			}
			screenRect.y += 28f;
		}
		GUI.EndGroup();
		GUI.EndGroup();
	}

	private string LabelOf(PrisonerInteractionMode mode)
	{
		return mode switch
		{
			PrisonerInteractionMode.NoInteraction => "No interaction", 
			PrisonerInteractionMode.FriendlyChat => "Friendly chat", 
			PrisonerInteractionMode.BeatingMild => "Mild beating", 
			PrisonerInteractionMode.BeatingVicious => "Vicious beating", 
			PrisonerInteractionMode.Execution => "Execution", 
			_ => "Mode needs label", 
		};
	}
}
