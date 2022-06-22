using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ThingDefDatabase
{
	private static List<ThingDefinition> allThingDefs;

	private static Dictionary<string, ThingDefinition> allThingDefsByName;

	public static List<ThingDefinition> AllThingDefinitions => allThingDefs;

	static ThingDefDatabase()
	{
		allThingDefs = new List<ThingDefinition>();
		allThingDefsByName = new Dictionary<string, ThingDefinition>();
		ThingDefinitionLoader.LoadThingDefinitions();
	}

	public static ThingDefinition DefinitionOfType(this EntityType t)
	{
		ThingDefinition thingDefinition = allThingDefs.Where((ThingDefinition d) => d.eType == t).FirstOrDefault();
		if (thingDefinition == null)
		{
			Debug.LogWarning("Could not find def of type " + t);
		}
		return thingDefinition;
	}

	public static ThingDefinition ThingDefNamed(string defName)
	{
		if (allThingDefsByName.TryGetValue(defName, out var value))
		{
			return value;
		}
		Debug.LogError("Could not find ThingDefinition named '" + defName + "'.");
		return null;
	}

	public static bool HaveThingDefNamed(string name)
	{
		return allThingDefsByName.ContainsKey(name);
	}

	public static void AddDefinitionToDatabase(ThingDefinition newDef)
	{
		if (HaveThingDefNamed(newDef.definitionName))
		{
			Debug.LogError("Already have thing def named " + newDef.definitionName);
			return;
		}
		allThingDefs.Add(newDef);
		allThingDefsByName.Add(newDef.definitionName, newDef);
	}
}
