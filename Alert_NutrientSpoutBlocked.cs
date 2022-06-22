public class Alert_NutrientSpoutBlocked : Alert
{
	public override AlertReport Report
	{
		get
		{
			foreach (Building_NutrientDispenser item in Find.BuildingManager.AllBuildingsColonistOfClass<Building_NutrientDispenser>())
			{
				if (!item.InteractionSquare.Standable())
				{
					return AlertReport.CulpritIs(item);
				}
			}
			return AlertReport.Inactive;
		}
	}

	public Alert_NutrientSpoutBlocked()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Nutrient spout blocked";
		baseExplanation = "You have a nutrient dispenser with its spout blocked.\n\nClear a space at the spout so people can actually use the nutrient dispenser.";
	}
}
