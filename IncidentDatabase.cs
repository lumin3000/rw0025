using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IncidentDatabase
{
	public static List<IncidentDefinition> allIncidentDefs;

	public static IEnumerable<IncidentDefinition> GlobalQueueableIncidents => allIncidentDefs.Where((IncidentDefinition def) => def.global);

	static IncidentDatabase()
	{
		allIncidentDefs = new List<IncidentDefinition>();
		foreach (Type item2 in typeof(IncidentDefinition).AllLeafSubclasses())
		{
			IncidentDefinition item = (IncidentDefinition)Activator.CreateInstance(item2);
			allIncidentDefs.Add(item);
		}
		foreach (IncidentDefinition allIncidentDef in allIncidentDefs)
		{
			foreach (IncidentDefinition allIncidentDef2 in allIncidentDefs)
			{
				if (allIncidentDef != allIncidentDef2 && allIncidentDef.uniqueSaveKey == allIncidentDef2.uniqueSaveKey)
				{
					Debug.LogError("Two incidents share unique save key " + allIncidentDef.uniqueSaveKey);
				}
			}
		}
	}

	public static IncidentDefinition IncidentDefOfType(Type iType)
	{
		foreach (IncidentDefinition allIncidentDef in allIncidentDefs)
		{
			if (iType == allIncidentDef.GetType())
			{
				return allIncidentDef;
			}
		}
		Debug.LogError("Missing Incident definition for " + iType);
		return null;
	}

	public static IncidentDefinition RandomBigThreat()
	{
		return allIncidentDefs.Where((IncidentDefinition def) => def.threatLevel >= IncidentThreatLevel.BigThreat).ToList().RandomElementByWeight((IncidentDefinition def) => def.chance);
	}

	public static IncidentDefinition RandomSmallThreat()
	{
		return allIncidentDefs.Where((IncidentDefinition def) => def.threatLevel == IncidentThreatLevel.SmallThreat).ToList().RandomElementByWeight((IncidentDefinition def) => def.chance);
	}
}
