public class Alert_RoomNeedsAirOutlet : Alert
{
	public override AlertReport Report
	{
		get
		{
			if (DebugSettings.worldBreathable)
			{
				return false;
			}
			if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_AirOutlet))
			{
				return AlertReport.Inactive;
			}
			foreach (Room allRoom in Find.RoomManager.allRooms)
			{
				if (!allRoom.ContainsThingOfType(EntityType.Building_AirOutlet))
				{
					return AlertReport.Active;
				}
			}
			return AlertReport.Inactive;
		}
	}

	public Alert_RoomNeedsAirOutlet()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need air outlet";
		baseExplanation = "You have no air outlet in your colony. Build an air outlet inside or it will never be pressurized.";
	}
}
