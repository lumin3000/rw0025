using UnityEngine;

public class ITab_Pawn_Character : ITab
{
	private const float ItemHeight = 30f;

	public override bool IsVisible => base.SelPawn.story != null;

	public ITab_Pawn_Character()
	{
		Size = PawnCardUtility.PawnCardSize + new Vector2(17f, 17f) * 2f;
		Label = "Character";
	}

	protected override void FillTab()
	{
		Rect position = new Rect(17f, 17f, PawnCardUtility.PawnCardSize.x, PawnCardUtility.PawnCardSize.y);
		GUI.BeginGroup(position);
		PawnCardUtility.DrawPawnCard(base.SelPawn);
		GUI.EndGroup();
	}
}
