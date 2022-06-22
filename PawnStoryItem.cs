using System;
using System.Collections.Generic;
using System.Text;

public class PawnStoryItem
{
	public string title = "Untitled";

	public string titleShort = "Untitled short";

	public string baseDesc = "No description.";

	public CharHistorySlot slot;

	public Dictionary<SkillType, int> skillGains = new Dictionary<SkillType, int>();

	public WorkTags workDisables;

	public List<CharHistoryCategory> categories = new List<CharHistoryCategory>();

	public Dictionary<Gender, string> bodyGraphicNames = new Dictionary<Gender, string>();

	public IEnumerable<WorkType> DisabledWorkTypes
	{
		get
		{
			foreach (int work in Enum.GetValues(typeof(WorkType)))
			{
				if (!AllowsWorkType((WorkType)work))
				{
					yield return (WorkType)work;
				}
			}
		}
	}

	public string FullDescriptionFor(Pawn p)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(GenText.TextAdjustedFor(p, baseDesc));
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		foreach (int value in Enum.GetValues(typeof(SkillType)))
		{
			if (skillGains.ContainsKey((SkillType)value))
			{
				stringBuilder.AppendLine(((SkillType)value).GetDefinition().label + ":   " + skillGains[(SkillType)value].ToString("+##;-##"));
			}
		}
		foreach (WorkType disabledWorkType in DisabledWorkTypes)
		{
			stringBuilder.AppendLine(disabledWorkType.GetDefinition().gerundLabel + " disabled");
		}
		return stringBuilder.ToString();
	}

	public bool AllowsWorkType(WorkType work)
	{
		WorkDefinition workDefinition = WorkDefDatabase.DefinitionOf(work);
		return (workDisables & workDefinition.workTags) == 0;
	}
}
