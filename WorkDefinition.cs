using System.Collections.Generic;

public class WorkDefinition
{
	public WorkType wType;

	public WorkTags workTags;

	public string pawnLabel = "NO PAWN LABEL";

	public string gerundLabel = "NO GERUND LABEL";

	public string verb = "NO VERB";

	public string tooltipDesc = "Work needs tooltip.";

	public bool automatic = true;

	public int naturalPriority;

	public bool startActive;

	public List<SkillType> relevantSkills = new List<SkillType>();

	public bool emergency;
}
