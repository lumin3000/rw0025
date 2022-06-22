public class Alert_NeedBatteries : Alert
{
	public override string FullExplanation => "You only have solar generators and no batteries. This means you'll lose power every time the sun goes down.\n\nBuild a battery or two to get you through the night.";

	public override AlertReport Report
	{
		get
		{
			if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_Battery))
			{
				return false;
			}
			if (!Find.BuildingManager.PlayerHasBuildingOfDef(ThingDefDatabase.ThingDefNamed("SolarGenerator")))
			{
				return false;
			}
			if (Find.BuildingManager.PlayerHasBuildingOfDef(ThingDefDatabase.ThingDefNamed("GeothermalGenerator")))
			{
				return false;
			}
			return true;
		}
	}

	public Alert_NeedBatteries()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need batteries";
	}
}
