using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RoofDefDatabase
{
	public static List<RoofDefinition> allRoofDefs;

	static RoofDefDatabase()
	{
		allRoofDefs = new List<RoofDefinition>();
		RoofDefinition item = new RoofDefinition
		{
			eType = EntityType.Roof_Metal,
			label = "Metal roof (thin)",
			vanishOnCollapse = true,
			collapseLeavingDefinitionName = "SlagDebris",
			filthLeavings = { { "SlagRubble", 2 } }
		};
		allRoofDefs.Add(item);
		item = new RoofDefinition
		{
			eType = EntityType.Roof_RockThin,
			label = "Rock roof (thin)",
			vanishOnCollapse = true,
			collapseLeavingDefinitionName = "RockDebris",
			filthLeavings = { { "RockRubble", 2 } }
		};
		allRoofDefs.Add(item);
		item = new RoofDefinition
		{
			eType = EntityType.Roof_RockThick,
			label = "Rock roof (thick)",
			vanishOnCollapse = false,
			collapseLeavingDefinitionName = "Rock",
			filthLeavings = { { "RockRubble", 2 } }
		};
		allRoofDefs.Add(item);
	}

	public static RoofDefinition RoofDefOfType(EntityType roofType)
	{
		if (roofType == EntityType.Undefined)
		{
			return null;
		}
		RoofDefinition roofDefinition = allRoofDefs.Where((RoofDefinition r) => r.eType == roofType).FirstOrDefault();
		if (roofDefinition == null)
		{
			Debug.LogError("Failed to find roof def of type " + roofType);
		}
		return roofDefinition;
	}
}
