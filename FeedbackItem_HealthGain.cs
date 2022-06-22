using UnityEngine;

public class FeedbackItem_HealthGain : FeedbackItem
{
	protected Pawn Healer;

	protected int Amount;

	public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer)
		: base(ScreenPos)
	{
		this.Amount = Amount;
		this.Healer = Healer;
	}

	public override void FeedbackOnGUI()
	{
		string empty = string.Empty;
		empty = ((Amount < 0) ? "-" : "+");
		empty += Amount;
		DrawFloatingText(empty, Color.red);
	}
}
