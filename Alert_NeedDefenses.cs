using System.Text;

public class Alert_NeedDefenses : Alert
{
	public override string FullLabel => "Need defenses";

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("You've been here some time and have probably been seen. Pirate raids will start soon.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("You should prepare defenses.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("Build some auto-turrets.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report
	{
		get
		{
			if (DateHandler.DaysPassed < 3)
			{
				return false;
			}
			if (DateHandler.DaysPassed >= 10)
			{
				return false;
			}
			if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_TurretGun))
			{
				return false;
			}
			return true;
		}
	}

	public Alert_NeedDefenses()
	{
		basePriority = AlertPriority.High;
	}
}
