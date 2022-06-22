public class Alert_LowFood : Alert
{
	public override string FullExplanation => "You only have " + Find.ResourceManager.Food + " food in storage. This is dangerously low.\n\nGrow, buy, or find some food.";

	public override AlertReport Report => Find.ResourceManager.Food < 40;

	public Alert_LowFood()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Low food";
	}
}
