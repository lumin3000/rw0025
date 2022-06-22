using System.Text;

public class Alert_MentalBreakImminent : Alert
{
	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These colonists are critically stressed and may have a mental break at any moment:");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Pawn colonist in Find.PawnManager.Colonists)
			{
				if (colonist.psychology.MentalBreakImminent)
				{
					stringBuilder.Append("    " + colonist.characterName);
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Improve these colonists' psychology immediately or bad things will happen.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report
	{
		get
		{
			foreach (Pawn colonist in Find.PawnManager.Colonists)
			{
				if (colonist.psychology.MentalBreakImminent)
				{
					return AlertReport.CulpritIs(colonist);
				}
			}
			return AlertReport.Inactive;
		}
	}

	public Alert_MentalBreakImminent()
	{
		basePriority = AlertPriority.Critical;
		baseLabel = "Mental break imminent";
	}
}
