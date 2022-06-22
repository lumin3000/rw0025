using System.Collections.Generic;

public static class SeedDefinitionMaker
{
	public static IEnumerable<ThingDefinition> SeedDefinitions()
	{
		foreach (ThingDefinition plantDef in ThingDefDatabase.AllThingDefinitions.ListFullCopy())
		{
			if (plantDef.IsPlant && plantDef.plant.SeedEmitAveragePer20kTicks > 0f)
			{
				ThingDefinition seedDef = ThingDefBases.NewBaseDefinitionFrom(EntityType.Proj_Seed);
				seedDef.definitionName = plantDef.definitionName + "_Seed";
				seedDef.label = plantDef.label + " seed";
				seedDef.seed_PlantDefToMake = plantDef;
				plantDef.plant.seedDefinition = seedDef;
				yield return seedDef;
			}
		}
	}
}
