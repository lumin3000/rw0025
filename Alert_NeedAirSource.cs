public class Alert_NeedAirSource : Alert
{
	public override AlertReport Report
	{
		get
		{
			if (DebugSettings.worldBreathable)
			{
				return false;
			}
			if (!Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_AirMiner))
			{
				return AlertReport.Active;
			}
			return AlertReport.Inactive;
		}
	}

	public Alert_NeedAirSource()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need air source";
		baseExplanation = "You have no air source.\n\nBuild an air miner outside to feed air to your buildings.";
	}
}
