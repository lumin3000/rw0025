using System.Collections.Generic;

public class ResearchProject : Saveable
{
	public ResearchType rType;

	public string label;

	public string descriptionBefore;

	public string descriptionDiscovered = string.Empty;

	public float totalCost;

	public List<ResearchType> researchPrereqs = new List<ResearchType>();

	public float progress;

	public float PercentComplete => progress / totalCost;

	public bool IsFinished => progress >= totalCost;

	public string DiscoveredText
	{
		get
		{
			if (descriptionDiscovered == string.Empty)
			{
				return descriptionBefore;
			}
			return descriptionDiscovered;
		}
	}

	public ResearchProject()
	{
	}

	public ResearchProject(ResearchType rType, float totalCost, string label, string desc)
	{
		this.rType = rType;
		this.label = label;
		this.totalCost = totalCost;
		descriptionBefore = desc;
		descriptionDiscovered = desc;
	}

	public ResearchProject(ResearchType rType, float totalCost, string label, string descBefore, string descDiscovered)
		: this(rType, totalCost, label, descBefore)
	{
		descriptionDiscovered = descDiscovered;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref rType, "RType", forceSave: true);
		Scribe.LookField(ref label, "Label", forceSave: true);
		Scribe.LookField(ref progress, "Progress", forceSave: true);
		Scribe.LookField(ref totalCost, "TotalCost", forceSave: true);
		Scribe.LookField(ref descriptionBefore, "Description", forceSave: true);
	}

	public bool PrereqsFulfilled()
	{
		foreach (ResearchType researchPrereq in researchPrereqs)
		{
			if (!Find.ResearchManager.HasResearched(researchPrereq))
			{
				return false;
			}
		}
		return true;
	}

	public string ProgressNumbersString()
	{
		return (progress / 10f).ToString("#######0") + " / " + (totalCost / 10f).ToString("#######0");
	}

	public override string ToString()
	{
		return string.Concat("ResearchProject ", rType, ": ", progress, "/", totalCost);
	}
}
