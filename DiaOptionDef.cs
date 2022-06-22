using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

public class DiaOptionDef
{
	[XmlAttribute]
	[DefaultValue("OK")]
	public string Text = "OK";

	[DefaultValue(null)]
	[XmlElement("Requirements")]
	public DiaRequirementSet ReqSet;

	[XmlElement("Node")]
	public List<DiaNodeDef> ChildNodes = new List<DiaNodeDef>();

	[XmlElement("NodeName")]
	[DefaultValue("")]
	public List<string> ChildNodeNames = new List<string>();

	[DefaultValue(null)]
	[XmlElement("Mission")]
	public MissionDef MissionToStart;

	public DiaNodeDef RandomLinkNode()
	{
		List<DiaNodeDef> list = ChildNodes.ListFullCopy();
		foreach (string childNodeName in ChildNodeNames)
		{
			list.Add(DialogDatabase.GetNodeNamed(childNodeName));
		}
		foreach (DiaNodeDef item in list)
		{
			if (item.Unique && item.Used)
			{
				list.Remove(item);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list.RandomElement();
	}
}
