using UnityEngine;

public class ITab_Pawn_Needs : ITab
{
	private const float ThoughtLevelHeight = 45f;

	private const float ThoughtLevelSpacing = 8f;

	public override bool IsVisible => base.SelPawn.food != null || base.SelPawn.rest != null;

	public ITab_Pawn_Needs()
	{
		Size = new Vector2(400f, 300f);
		Label = "Needs";
	}

	protected override void FillTab()
	{
		GenUI.SetFontSmall();
		Rect baseRect = new Rect(0f, 0f, Size.x, Size.y);
		Rect innerRect = baseRect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect);
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		Rect rect = new Rect(10f, 0f, innerRect.width - 20f, 45f);
		if (base.SelPawn.food != null)
		{
			GenUI.DrawThoughtLevel(base.SelPawn.food.Food, rect);
			rect.y += 53f;
		}
		if (base.SelPawn.rest != null)
		{
			GenUI.DrawThoughtLevel(base.SelPawn.rest.Rest, rect);
			rect.y += 53f;
		}
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.Label(rect, "Effectiveness: " + Gen.SplitCamelCase(base.SelPawn.healthTracker.CurEffectiveness.ToString()));
		Rect position = new Rect(180f, rect.yMax + 10f, innerRect.width - 180f, innerRect.height);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.Label(position, base.SelPawn.filth.FilthReport);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.EndGroup();
	}
}
