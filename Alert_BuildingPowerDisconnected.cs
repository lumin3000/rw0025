using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Alert_BuildingPowerDisconnected : Alert
{
	private IEnumerable<Building> BuildingsPowerDisconnected
	{
		get
		{
			foreach (Building b in Find.BuildingManager.AllBuildingsColonist)
			{
				if ((b.def.transmitsPower || b.def.ConnectToPower) && b.def.eType != EntityType.Building_PowerConduit && b.def.eType != EntityType.Wall && (b.powerNet == null || !b.powerNet.hasPowerSource) && (b.connectedToTransmitter == null || !b.connectedToTransmitter.powerNet.hasPowerSource))
				{
					BuildingFrame frame = b as BuildingFrame;
					if (frame == null)
					{
						yield return b;
					}
				}
			}
		}
	}

	public override string FullLabel
	{
		get
		{
			if (BuildingsPowerDisconnected.Count() == 1)
			{
				return "Building disconnected";
			}
			return "Buildings disconnected";
		}
	}

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These power-using buildings are totally disconnected from all possible power sources:");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			int num = 0;
			foreach (Building item in BuildingsPowerDisconnected)
			{
				if (num < 5)
				{
					stringBuilder.Append("    " + item.Label);
					num++;
					stringBuilder.AppendLine();
					continue;
				}
				stringBuilder.AppendLine("    (and more)");
				break;
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Connect them to a power generator or battery.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report => AlertReport.CulpritIs(BuildingsPowerDisconnected.FirstOrDefault());

	public Alert_BuildingPowerDisconnected()
	{
		basePriority = AlertPriority.High;
	}
}
