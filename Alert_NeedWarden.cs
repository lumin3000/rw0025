using System.Linq;

public class Alert_NeedWarden : Alert
{
	public override AlertReport Report
	{
		get
		{
			if (Find.PawnManager.PawnsOnTeam[TeamType.Prisoner].Count == 0)
			{
				return AlertReport.Inactive;
			}
			foreach (Pawn colonist in Find.PawnManager.Colonists)
			{
				if (!colonist.Incapacitated && colonist.WorkSettings.ActiveWorksByPriority.Contains(WorkType.Warden))
				{
					return AlertReport.Inactive;
				}
			}
			return AlertReport.CulpritIs(Find.PawnManager.PawnsOnTeam[TeamType.Prisoner][0]);
		}
	}

	public Alert_NeedWarden()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need warden";
		baseExplanation = "You have a prisoner, but no colonist has the Warden work type.\n\nAssign a colonist the Warden work type.";
	}
}
