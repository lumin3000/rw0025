using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Alert_ColonistNeedsRescuing : Alert
{
	private IEnumerable<Pawn> ColonistsNeedingRescue
	{
		get
		{
			foreach (Pawn p in Find.PawnManager.Colonists)
			{
				if (p.Incapacitated && !p.IsInBed())
				{
					yield return p;
				}
			}
		}
	}

	public override string FullLabel
	{
		get
		{
			if (ColonistsNeedingRescue.Count() == 1)
			{
				return "Colonist needs rescue";
			}
			return "Colonists need rescue";
		}
	}

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These colonists are incapacitated on the ground:");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Pawn item in ColonistsNeedingRescue)
			{
				stringBuilder.Append("    " + item.characterName);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Send another colonist to rescue them and carry them back to bed.\n\n(To rescue, select another colonist, then right click on the victim and select Rescue.)");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report => AlertReport.CulpritIs(ColonistsNeedingRescue.FirstOrDefault());

	public Alert_ColonistNeedsRescuing()
	{
		basePriority = AlertPriority.Critical;
	}
}
