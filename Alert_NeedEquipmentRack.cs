public class Alert_NeedEquipmentRack : Alert
{
	public override AlertReport Report
	{
		get
		{
			if (DateHandler.DaysPassed < 4)
			{
				return false;
			}
			if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_EquipmentRack))
			{
				return false;
			}
			return true;
		}
	}

	public Alert_NeedEquipmentRack()
	{
		basePriority = AlertPriority.Medium;
		baseLabel = "Need equipment rack";
		baseExplanation = "You have nowhere to store weapons and equipment. Build an equipment rack.";
	}
}
