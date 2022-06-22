using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WildAnimalMaker
{
	private const float DesiredAnimalsPerHundredSquare = 4f;

	private static List<PawnKindDefinition> wildAnimalDefs;

	private static float DesiredEcosystemWeight
	{
		get
		{
			int num = 2500;
			return Find.Map.Area / num;
		}
	}

	static WildAnimalMaker()
	{
		wildAnimalDefs = PawnKindDefDatabase.AllKindDefs.Where((PawnKindDefinition def) => def.wildSpawn_spawnWild).ToList();
	}

	public static void SpawnRandomWildAnimalAt(IntVec3 loc)
	{
		PawnKindDefinition pawnKindDefinition = wildAnimalDefs.RandomElementByWeight((PawnKindDefinition def) => def.wildSpawn_SelectionWeight / def.wildSpawn_GroupSizeRange.Average);
		int randomInRange = pawnKindDefinition.wildSpawn_GroupSizeRange.RandomInRange;
		int squareRadius = Mathf.CeilToInt(Mathf.Sqrt(pawnKindDefinition.wildSpawn_GroupSizeRange.max));
		for (int i = 0; i < randomInRange; i++)
		{
			IntVec3 newThingPos = GenMap.RandomStandableLOSSquareNear(loc, squareRadius);
			Pawn newThing = PawnMaker.GeneratePawn(pawnKindDefinition.kindLabel);
			ThingMaker.Spawn(newThing, newThingPos);
		}
	}

	public static bool EcosystemFull()
	{
		float num = 0f;
		foreach (Pawn allPawn in Find.PawnManager.AllPawns)
		{
			if (allPawn.kindDef.wildSpawn_spawnWild)
			{
				num += allPawn.kindDef.wildSpawn_EcoSystemWeight;
			}
		}
		return num >= DesiredEcosystemWeight;
	}
}
