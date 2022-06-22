using System;

public class Genner_Animals
{
	public void AddAnimals()
	{
		while (!WildAnimalMaker.EcosystemFull())
		{
			Predicate<IntVec3> validator = delegate(IntVec3 sq)
			{
				if (!sq.Standable())
				{
					return false;
				}
				if (MapGenerator.sanctity.SanctityAt(sq) > 0f)
				{
					return false;
				}
				return (!sq.Isolated()) ? true : false;
			};
			IntVec3 loc = GenMap.RandomSquareWith(validator);
			WildAnimalMaker.SpawnRandomWildAnimalAt(loc);
		}
	}
}
