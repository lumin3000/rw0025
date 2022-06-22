using System.Linq;

public class Alert_NeedMiner : Alert
{
	public override AlertReport Report
	{
		get
		{
			Designation designation = Find.DesignationManager.designationList.Where((Designation d) => d is Designation_Mine).FirstOrDefault();
			if (designation == null)
			{
				return AlertReport.Inactive;
			}
			foreach (Pawn colonist in Find.PawnManager.Colonists)
			{
				if (!colonist.Incapacitated && colonist.WorkSettings.ActiveWorksByPriority.Contains(WorkType.Mining))
				{
					return AlertReport.Inactive;
				}
			}
			return AlertReport.CulpritIs(designation.target.thing);
		}
	}

	public Alert_NeedMiner()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need miner";
		baseExplanation = "You have designated some rocks to be mined, but no colonist has the Mining work type.\n\nAssign a colonist the Mining work type.";
	}
}
