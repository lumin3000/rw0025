using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

[XmlRoot("Mission")]
public class MissionDef
{
	[DefaultValue("NeedsName")]
	[XmlAttribute]
	public string Name = "NeedsName";

	[XmlAttribute("Type")]
	public MissionType MissionType;

	[XmlAttribute]
	[DefaultValue(EnemyDisposition.Defensive)]
	public EnemyDisposition Attitude;

	[DefaultValue(-1)]
	[XmlAttribute]
	public int EnemyPoints = -1;

	[XmlElement("Map")]
	public List<string> MapList = new List<string>();

	[XmlElement("VictoryNode")]
	public List<DiaNodeDef> VicNodes = new List<DiaNodeDef>();

	[XmlElement("VictoryNodeName")]
	public List<string> VicNodeNames = new List<string>();

	public DiaNodeDef RandomVictoryNode()
	{
		List<DiaNodeDef> list = VicNodes.ListFullCopy();
		foreach (string vicNodeName in VicNodeNames)
		{
			list.Add(DialogDatabase.GetNodeNamed(vicNodeName));
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
