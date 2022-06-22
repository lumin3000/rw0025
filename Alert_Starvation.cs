using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Alert_Starvation : Alert
{
	private IEnumerable<Pawn> StarvingColonists => Find.PawnManager.Colonists.Where((Pawn p) => p.food.Food.Starving);

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These colonists are starving:");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Pawn starvingColonist in StarvingColonists)
			{
				stringBuilder.Append("    " + starvingColonist.characterName);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Get them some food. You'll need a food source like a nutrient paste dispenser and some food to feed it. Also, make sure they're not drafted.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report => AlertReport.CulpritIs(StarvingColonists.FirstOrDefault());

	public Alert_Starvation()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Starvation";
	}
}
