using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TerrainDefDatabase
{
	public static List<TerrainDefinition> allTerrainDefs;

	static TerrainDefDatabase()
	{
		allTerrainDefs = DataLoader.LoadDataInFolder<TerrainDefinition>("Terrain/Surfaces");
		HashSet<int> hashSet = new HashSet<int>();
		foreach (TerrainDefinition allTerrainDef in allTerrainDefs)
		{
			if (hashSet.Contains(allTerrainDef.renderPrecedence))
			{
				Debug.LogWarning("Duplicate use of render order " + allTerrainDef.renderPrecedence + " by terrain def " + allTerrainDef.label);
			}
			if (allTerrainDef.renderPrecedence > 255)
			{
				Debug.LogWarning("Render order of " + allTerrainDef.renderPrecedence + " is out of range ( " + allTerrainDef.renderPrecedence + " > 255)");
			}
		}
	}

	public static TerrainDefinition TerrainWithLabel(string terrName)
	{
		TerrainDefinition terrainDefinition = allTerrainDefs.Where((TerrainDefinition terr) => terr.label == terrName).FirstOrDefault();
		if (terrainDefinition == null)
		{
			Debug.LogError("Failed to find terrain with label " + terrName);
		}
		return terrainDefinition;
	}
}
