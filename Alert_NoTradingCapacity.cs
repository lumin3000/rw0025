public class Alert_NoTradingCapacity : Alert
{
	public override AlertReport Report
	{
		get
		{
			if (DateHandler.CyclesPassed < 1)
			{
				return AlertReport.Inactive;
			}
			if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_CommsConsole))
			{
				return AlertReport.Inactive;
			}
			return AlertReport.Active;
		}
	}

	public Alert_NoTradingCapacity()
	{
		basePriority = AlertPriority.Medium;
		baseLabel = "No trading capacity";
		baseExplanation = "You have no way of trading with passing ships.\n\nBuild a comms console and a landing area. You may need to research them first.";
	}
}
