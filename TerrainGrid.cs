using System.Collections.Generic;

public class TerrainGrid : Saveable
{
	public TerrainDefinition[,,] terrainGrid;

	public TerrainGrid()
	{
		ResetGrids();
	}

	public void ResetGrids()
	{
		terrainGrid = new TerrainDefinition[Find.Map.Size.x, Find.Map.Size.y, Find.Map.Size.z];
	}

	public TerrainDefinition TerrainAt(IntVec3 loc)
	{
		return terrainGrid[loc.x, loc.y, loc.z];
	}

	public void SetTerrain(IntVec3 loc, TerrainDefinition newTerr)
	{
		terrainGrid[loc.x, loc.y, loc.z] = newTerr;
		IntVec3[] cardinalDirections = Gen.CardinalDirections;
		foreach (IntVec3 intVec in cardinalDirections)
		{
			if ((loc + intVec).InBounds())
			{
				Find.Map.mapDrawer.MapChanged(loc + intVec, MapChangeType.Terrain);
			}
		}
		Plant plant = PlantUtility.PlantInSquare(loc);
		if (plant != null && Find.FertilityGrid.FertilityAt(loc) < plant.def.plant.minFertility)
		{
			plant.Destroy();
		}
		Find.PathGrid.RecalculatePathCostAt(loc);
	}

	public void ExposeData()
	{
		string value = string.Empty;
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			value = GridSaveUtility.CompressedStringForByteGrid((IntVec3 loc) => terrainGrid[loc.x, loc.y, loc.z].UniqueSaveKey);
		}
		Scribe.LookField(ref value, "TerrainMap");
		if (Scribe.mode != LoadSaveMode.LoadingVars)
		{
			return;
		}
		Dictionary<int, TerrainDefinition> dictionary = new Dictionary<int, TerrainDefinition>();
		foreach (TerrainDefinition allTerrainDef in TerrainDefDatabase.allTerrainDefs)
		{
			dictionary.Add(allTerrainDef.UniqueSaveKey, allTerrainDef);
		}
		foreach (GridSaveUtility.LoadedGridByte item in GridSaveUtility.ThingsFromThingTypeGrid(value))
		{
			terrainGrid[item.pos.x, item.pos.y, item.pos.z] = dictionary[item.val];
		}
	}
}
