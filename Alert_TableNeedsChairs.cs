using System.Collections.Generic;
using System.Linq;

public class Alert_TableNeedsChairs : Alert
{
	public override AlertReport Report
	{
		get
		{
			IEnumerable<Building> enumerable = Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Building_TableShort).Concat(Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Building_TableLong));
			foreach (Building item in enumerable)
			{
				bool flag = false;
				foreach (IntVec3 item2 in Gen.AdjacentSquaresCardinal(item))
				{
					if (Find.Grids.SquareContains(item2, EntityType.Building_Chair))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return AlertReport.CulpritIs(item);
				}
			}
			return AlertReport.Inactive;
		}
	}

	public Alert_TableNeedsChairs()
	{
		basePriority = AlertPriority.Medium;
		baseLabel = "Table needs chairs";
		baseExplanation = "You have a table with no chairs next to it. Tables are useless unless people can sit at them.";
	}
}
