using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

public class ResourceModifyDef
{
	[XmlAttribute("Type")]
	public string resType;

	[DefaultValue(0)]
	[XmlAttribute("Amount")]
	public int Amount;

	[DefaultValue(0)]
	[XmlAttribute("Min")]
	public int AmountMin;

	[DefaultValue(0)]
	[XmlAttribute("Max")]
	public int AmountMax;

	public void DoResourceModification()
	{
		int num = Amount;
		if (Amount == 0)
		{
			num = Random.Range(AmountMin, AmountMax);
		}
		if (resType == "Food")
		{
			Find.ResourceManager.Food += num;
		}
		if (resType == "Medicine")
		{
			Find.ResourceManager.Medicine += num;
		}
	}
}
