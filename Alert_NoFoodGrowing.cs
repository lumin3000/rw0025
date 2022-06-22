public class Alert_NoFoodGrowing : Alert
{
	public override AlertReport Report
	{
		get
		{
			if (DateHandler.DaysPassed < 4)
			{
				return AlertReport.Inactive;
			}
			if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Area_Growing))
			{
				return AlertReport.Inactive;
			}
			if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_HydroponicsTable))
			{
				return AlertReport.Inactive;
			}
			return AlertReport.Active;
		}
	}

	public Alert_NoFoodGrowing()
	{
		basePriority = AlertPriority.Medium;
		baseLabel = "No food growing";
		baseExplanation = "You have no way of growing food.\n\nPlace a growing area on soil and your colonists will sow crops.";
	}
}
