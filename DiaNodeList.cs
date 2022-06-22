using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("NodeList")]
public class DiaNodeList
{
	[XmlAttribute]
	public string Name = "NeedsName";

	[XmlElement("Node")]
	public List<DiaNodeDef> Nodes = new List<DiaNodeDef>();

	[XmlElement("NodeName")]
	public List<string> NodeNames = new List<string>();

	public DiaNodeDef RandomNodeFromList()
	{
		List<DiaNodeDef> list = Nodes.ListFullCopy();
		foreach (string nodeName in NodeNames)
		{
			list.Add(DialogDatabase.GetNodeNamed(nodeName));
		}
		foreach (DiaNodeDef item in list)
		{
			if (item.Unique && item.Used)
			{
				list.Remove(item);
			}
		}
		return list.RandomElement();
	}
}
