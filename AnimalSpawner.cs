using UnityEngine;

public class AnimalSpawner : Saveable
{
	private const int CheckInterval = 500;

	private const float AnimalSpawnChancePerTick = 0.001f;

	public void ExposeData()
	{
	}

	public void AnimalSpawnerTick()
	{
		if (Find.TickManager.tickCount % 500 == 0 && !WildAnimalMaker.EcosystemFull() && Random.value < 0.5f)
		{
			IntVec3 loc = GenMap.RandomEdgeSquareWith((IntVec3 sq) => sq.Standable() && !sq.Isolated());
			WildAnimalMaker.SpawnRandomWildAnimalAt(loc);
		}
	}
}
