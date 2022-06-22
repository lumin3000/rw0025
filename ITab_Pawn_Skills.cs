using UnityEngine;

public class ITab_Pawn_Skills : ITab
{
	public const float TabWidth = 300f;

	public static readonly Vector2 DisplayOffset = new Vector2(15f, 40f);

	public override bool IsVisible => base.SelPawn.skills != null;

	public ITab_Pawn_Skills()
	{
		Size = new Vector2(300f, 400f);
		Label = "Skills";
	}

	protected override void FillTab()
	{
		SkillUI.DrawSkillsOf(base.SelPawn, DisplayOffset, SkillUI.SkillDrawMode.Gameplay);
	}
}
