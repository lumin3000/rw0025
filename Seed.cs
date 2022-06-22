using UnityEngine;

public class Seed : Projectile
{
	protected override void Impact(Thing hitThing)
	{
		if (def.seed_PlantDefToMake.CanPlantAt(base.Position))
		{
			float to = Find.FertilityGrid.FertilityAt(base.Position);
			float num = Mathf.Lerp(1f, to, def.seed_PlantDefToMake.plant.fertilityFactorPlantChance);
			if (Random.value < num)
			{
				Plant plant = (Plant)ThingMaker.Spawn(def.seed_PlantDefToMake, base.Position);
				plant.growthPercent = 0.05f;
			}
		}
		Destroy();
	}
}
