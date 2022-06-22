public class Alert_NeedMealSource : Alert
{
	public override AlertReport Report
	{
		get
		{
			foreach (Building item in Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Building_NutrientDispenser))
			{
				if (!item.Position.IsInPrisonCell())
				{
					return false;
				}
			}
			return true;
		}
	}

	public Alert_NeedMealSource()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need meal source";
		baseExplanation = "You have no way of producing edible meals.\n\nBuild a nutrient paste dispenser.\n\n(It cannot be in a prison cell, otherwise colonists cannot use it.)";
	}
}
