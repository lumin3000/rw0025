using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Alert_NeedDoctor : Alert
{
	private IEnumerable<Pawn> HungryPatients => Find.PawnManager.Colonists.Where((Pawn p) => p.food.Food.CurHungerLevel > HungerLevel.Fed && p.Incapacitated && p.IsInBed());

	public override string FullExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("These patients are incapacitated in bed and hungry. They need to be fed, but you have no healthy colonists assigned to the Doctor work type.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (Pawn hungryPatient in HungryPatients)
			{
				stringBuilder.Append("    " + hungryPatient.characterName);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Assign a healthy colonist to the Doctor work type.");
			return stringBuilder.ToString();
		}
	}

	public override AlertReport Report
	{
		get
		{
			foreach (Pawn colonist in Find.PawnManager.Colonists)
			{
				if (!colonist.Incapacitated && colonist.WorkSettings.ActiveWorksByPriority.Contains(WorkType.Doctor))
				{
					return AlertReport.Inactive;
				}
			}
			return AlertReport.CulpritIs(HungryPatients.FirstOrDefault());
		}
	}

	public Alert_NeedDoctor()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need doctor";
	}
}
