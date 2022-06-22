using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Alert_BuildingNeedsAir : Alert
{
	private IEnumerable<Building> BuildingsNeedingAir
	{
		get
		{
			foreach (Building b in Find.BuildingManager.AllBuildingsColonist)
			{
				if (b.def.buildingWantsAir && !b.HasAir())
				{
					yield return b;
				}
			}
		}
	}

	public override string FullLabel
	{
		get
		{
			if (BuildingsNeedingAir.Count() == 1)
			{
				return "Building needs air";
			}
			return "Buildings need air";
		}
	}

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These buildings won't work unless they're enclosed in a pressurized room:");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Building item in BuildingsNeedingAir)
			{
				stringBuilder.Append("    " + item.Label);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Build a room around them and make sure it has air pressure.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report => AlertReport.CulpritIs(BuildingsNeedingAir.FirstOrDefault());

	public Alert_BuildingNeedsAir()
	{
		basePriority = AlertPriority.High;
	}
}
