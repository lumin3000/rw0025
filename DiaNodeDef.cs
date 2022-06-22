using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

[XmlRoot("Node")]
public class DiaNodeDef
{
	[XmlAttribute]
	[DefaultValue("Unnamed")]
	public string Name = "Unnamed";

	[DefaultValue(RewardCode.Undefined)]
	[XmlAttribute]
	public RewardCode Reward;

	[DefaultValue(false)]
	[XmlAttribute]
	public bool Unique;

	[XmlElement("Text")]
	public List<string> Texts = new List<string>();

	[XmlElement("ResourceModify")]
	[DefaultValue(null)]
	public ResourceModifyDefSet ResourceModifications;

	[XmlElement("Option")]
	public List<DiaOptionDef> OptionList = new List<DiaOptionDef>();

	[XmlIgnore]
	public bool IsRoot = true;

	[XmlIgnore]
	public bool Used;

	[XmlIgnore]
	public DiaNodeType NodeType;

	public void PostLoad()
	{
		int num = 0;
		foreach (string item in Texts.ListFullCopy())
		{
			Texts[num] = item.Replace("\\n", Environment.NewLine);
			num++;
		}
		foreach (DiaOptionDef option in OptionList)
		{
			foreach (DiaNodeDef childNode in option.ChildNodes)
			{
				childNode.PostLoad();
			}
		}
	}
}
