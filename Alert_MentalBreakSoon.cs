using System.Text;

public class Alert_MentalBreakSoon : Alert
{
	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These colonists are approaching the point of having a mental break:");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Pawn colonist in Find.PawnManager.Colonists)
			{
				if (colonist.psychology.MentalBreakApproaching)
				{
					stringBuilder.Append("    " + colonist.characterName);
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Improve these colonists' psychology soon or bad things will happen.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report
	{
		get
		{
			foreach (Pawn colonist in Find.PawnManager.Colonists)
			{
				if (colonist.psychology.MentalBreakApproaching)
				{
					return AlertReport.CulpritIs(colonist);
				}
			}
			return AlertReport.Inactive;
		}
	}

	public Alert_MentalBreakSoon()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Mental break soon";
	}
}
