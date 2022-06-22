using System;
using UnityEngine;

public class PlantReproducer
{
	public Plant plant;

	public PlantReproducer(Plant plant)
	{
		this.plant = plant;
	}

	public void PlantReproducerTickRare()
	{
		if (plant.growthPercent > plant.def.plant.SeedShootMinGrowthPercent && UnityEngine.Random.value < 250f * plant.def.plant.SeedEmitAveragePer20kTicks / 20000f)
		{
			SpawnSeed();
		}
	}

	private void SpawnSeed()
	{
		bool succeeded;
		IntVec3 intVec = SeedTargFor(plant.def, plant.Position, out succeeded);
		if (succeeded)
		{
			Seed seed = (Seed)ThingMaker.MakeThing(plant.def.plant.seedDefinition);
			ThingMaker.Spawn(seed, plant.Position, IntRot.random);
			seed.Launch(intVec);
			if (DebugSettings.fastEcology)
			{
				seed.ForceInstantImpact();
			}
		}
	}

	public static IntVec3 SeedTargFor(ThingDefinition plantDef, IntVec3 plantLoc, out bool succeeded)
	{
		Predicate<IntVec3> validator = delegate(IntVec3 sq)
		{
			if (!plantLoc.WithinHorizontalDistanceOf(sq, plantDef.plant.SeedShootRadius))
			{
				return false;
			}
			return GenGrid.LineOfSight(plantLoc, sq, skipFirstSquare: true) ? true : false;
		};
		return GenMap.RandomMapSquareNear(plantLoc, Mathf.CeilToInt(plantDef.plant.SeedShootRadius), validator, out succeeded);
	}
}
