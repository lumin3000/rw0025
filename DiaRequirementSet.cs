using System.Collections.Generic;
using System.Xml.Serialization;

public class DiaRequirementSet
{
	[XmlElement("Requirement")]
	public List<DiaRequirement> ReqList = new List<DiaRequirement>();

	public bool RequirementsAreSatisfied()
	{
		foreach (DiaRequirement req in ReqList)
		{
			if (!req.RequirementIsSatisfied())
			{
				return false;
			}
		}
		return true;
	}

	public string DissatisfactionReason()
	{
		foreach (DiaRequirement req in ReqList)
		{
			if (!req.RequirementIsSatisfied())
			{
				return req.DissatisfactionReason();
			}
		}
		return "[Error; no dissatisfaction reason in DiaRequirementSet]";
	}
}
