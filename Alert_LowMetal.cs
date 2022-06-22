public class Alert_LowMetal : Alert
{
	public override string FullExplanation => "You only have " + Find.ResourceManager.Metal + " metal in storage. This is dangerously low.\n\nTo get more metal, mine minerals (not rocks) or buy it from traders.";

	public override AlertReport Report => Find.ResourceManager.Metal < 50;

	public Alert_LowMetal()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Low metal";
	}
}
