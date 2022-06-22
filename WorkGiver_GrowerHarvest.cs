using System.Collections;

public class WorkGiver_GrowerHarvest : WorkGiver
{
	public override IEnumerable PotentialWorkTargets
	{
		get
		{
			foreach (Plant spawnedCultivatedPlant in Find.Map.thingLister.spawnedCultivatedPlants)
			{
				yield return spawnedCultivatedPlant;
			}
		}
	}

	public WorkGiver_GrowerHarvest(Pawn newPawn)
		: base(newPawn)
	{
		wType = WorkType.Growing;
	}

	public override Job StartingJobOn(Thing t)
	{
		Plant plant = t as Plant;
		if (plant == null)
		{
			return null;
		}
		if (plant.def.plant.Harvestable && plant.LifeStage == PlantLifeStage.Mature && pawn.CanReserve(plant, ReservationType.Total))
		{
			return new Job(JobType.Harvest, new TargetPack(plant));
		}
		return null;
	}
}
