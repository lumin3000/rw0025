using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Alert_ColonistsIdle : Alert
{
	private IEnumerable<Pawn> IdleColonists => Find.PawnManager.Colonists.Where((Pawn p) => p.MindState.IsIdle);

	public override string FullLabel => IdleColonists.Count() + " colonists idle";

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These colonists are wandering idly:");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Pawn idleColonist in IdleColonists)
			{
				stringBuilder.Append("    " + idleColonist.characterName);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("You should probably find them something productive to do.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report
	{
		get
		{
			if (DateHandler.DaysPassed < 1)
			{
				return AlertReport.Inactive;
			}
			return IdleColonists.FirstOrDefault();
		}
	}

	public Alert_ColonistsIdle()
	{
		basePriority = AlertPriority.Medium;
	}
}
