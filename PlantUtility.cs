using UnityEngine;

public static class PlantUtility
{
	public static bool CanPlantAt(this ThingDefinition plantDef, IntVec3 sq)
	{
		if (!plantDef.IsPlant)
		{
			Debug.LogError(string.Concat("Checking CanGrowAt with ", plantDef, " which is not a plant."));
		}
		if (Find.FertilityGrid.FertilityAt(sq) < plantDef.plant.minFertility)
		{
			return false;
		}
		foreach (Thing item in Find.Grids.ThingsAt(sq))
		{
			if (item.def.BlockPlanting)
			{
				return false;
			}
		}
		return true;
	}

	public static Plant PlantInSquare(IntVec3 sq)
	{
		foreach (Thing item in Find.Grids.ThingsAt(sq))
		{
			if (item.def.IsPlant)
			{
				return (Plant)item;
			}
		}
		return null;
	}
}
