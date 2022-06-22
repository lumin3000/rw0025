using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Genner_Plants
{
	private const float PlantDensity = 0.5f;

	private const float PlantMinGrowth = 0.07f;

	private const float PlantGrowthFactor = 1.2f;

	public void AddPlants()
	{
		List<ThingDefinition> source = ThingDefDatabase.AllThingDefinitions.Where((ThingDefinition def) => def.IsPlant && def.plant.wild).ToList();
		IntVec3 sq;
		foreach (IntVec3 allSquare in Find.Map.AllSquares)
		{
			sq = allSquare;
			if (Find.Grids.BlockerAt(sq) != null)
			{
				continue;
			}
			float num = Find.FertilityGrid.FertilityAt(sq);
			float num2 = num * 0.5f;
			if (Random.value >= num2)
			{
				continue;
			}
			List<ThingDefinition> list = source.Where((ThingDefinition def) => def.CanPlantAt(sq)).ToList();
			if (list.Count == 0)
			{
				continue;
			}
			ThingDefinition thingDefinition = list.RandomElementByWeight((ThingDefinition def) => def.plant.wildCommonality / def.plant.generateClusterSizeRange.Average);
			int randomInRange = thingDefinition.plant.generateClusterSizeRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 intVec;
				if (i == 0)
				{
					intVec = sq;
				}
				else
				{
					intVec = PlantReproducer.SeedTargFor(thingDefinition, sq, out var succeeded);
					if (!succeeded)
					{
						break;
					}
				}
				if (thingDefinition.CanPlantAt(intVec))
				{
					Plant plant = (Plant)ThingMaker.Spawn(thingDefinition, intVec);
					float num3 = Mathf.Lerp(0.07f, num * 1.2f, Random.value);
					if (num3 > 1f)
					{
						num3 = 1f;
					}
					plant.growthPercent = num3;
				}
			}
		}
	}
}
