using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Alert_DraftedColonistNeedsRelease : Alert
{
	private IEnumerable<Pawn> DraftedColonistsNeedy
	{
		get
		{
			foreach (Pawn p in Find.PawnManager.Colonists)
			{
				if (!p.Incapacitated && p.MindHuman.drafted && ((p.rest != null && p.rest.Rest.CurFatigueLevel >= FatigueLevel.VeryTired) || (p.food != null && p.food.Food.CurHungerLevel >= HungerLevel.UrgentlyHungry)))
				{
					yield return p;
				}
			}
		}
	}

	public override AlertReport Report => AlertReport.CulpritIs(DraftedColonistsNeedy.FirstOrDefault());

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These colonists are drafted under military orders, but are also very tired and/or hungry.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Pawn item in DraftedColonistsNeedy)
			{
				stringBuilder.Append("    " + item.characterName);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Consider releasing them from service so they can go fulfill their needs.");
			return stringBuilder.ToString();
		}
	}

	public Alert_DraftedColonistNeedsRelease()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Soldier needs break";
	}
}
