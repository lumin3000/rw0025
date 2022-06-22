using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

public class DiaRequirement
{
	[XmlAttribute("Type")]
	public string ReqType;

	[XmlAttribute("Amount")]
	[DefaultValue(0)]
	public int Amount;

	public bool RequirementIsSatisfied()
	{
		if (ReqType == "Food")
		{
			return Find.ResourceManager.Food >= Amount;
		}
		if (ReqType == "Medicine")
		{
			return Find.ResourceManager.Medicine >= Amount;
		}
		Debug.LogWarning("RequirementIsSatisfied hit end of method.");
		return false;
	}

	public string DissatisfactionReason()
	{
		if (ReqType == "Food")
		{
			return "Not enough food";
		}
		if (ReqType == "Medicine")
		{
			return "Not enough medicine";
		}
		return "[Error; no dissatisfaction reason in DiaRequirement]";
	}
}
