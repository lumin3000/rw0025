using System.Linq;

public class Alert_NeedColonistBeds : Alert
{
	public override AlertReport Report
	{
		get
		{
			int num = (from bed in Find.BuildingManager.AllBuildingsColonistOfClass<Building_Bed>()
				where !bed.forPrisoners
				select bed).Count();
			return num < Find.PawnManager.Colonists.Count;
		}
	}

	public Alert_NeedColonistBeds()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need colonist beds";
		baseExplanation = "You have more colonists than you have colonist beds. Someone will lack a place to sleep.\n\nEither make more beds, or change a prisoner bed to a colonist bed.";
	}
}
