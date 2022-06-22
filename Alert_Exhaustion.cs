using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Alert_Exhaustion : Alert
{
	private IEnumerable<Pawn> ExhaustedColonists => Find.PawnManager.Colonists.Where((Pawn p) => p.rest.Rest.CurFatigueLevel == FatigueLevel.Exhausted);

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These colonists are exhausted and need rest:");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Pawn exhaustedColonist in ExhaustedColonists)
			{
				stringBuilder.Append("    " + exhaustedColonist.characterName);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Get them some rest. They'll need a bed marked for colonist use. Also, make sure they're not drafted.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report => AlertReport.CulpritIs(ExhaustedColonists.FirstOrDefault());

	public Alert_Exhaustion()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Exhaustion";
	}
}
