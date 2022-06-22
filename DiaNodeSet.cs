using System.Collections.Generic;
using System.Xml.Serialization;

public class DiaNodeSet
{
	[XmlElement("Node")]
	public List<DiaNodeDef> NodeDefList = new List<DiaNodeDef>();

	[XmlElement("NodeList")]
	public List<DiaNodeList> NodeDefListList = new List<DiaNodeList>();

	[XmlElement("Mission")]
	public List<MissionDef> MissionDefList = new List<MissionDef>();
}
