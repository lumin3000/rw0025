using System.Collections.Generic;
using System.Xml.Serialization;

public class ResourceModifyDefSet
{
	[XmlElement("Resource")]
	public List<ResourceModifyDef> ResourceList = new List<ResourceModifyDef>();

	public void DoResourceModifications()
	{
		foreach (ResourceModifyDef resource in ResourceList)
		{
			resource.DoResourceModification();
		}
	}
}
