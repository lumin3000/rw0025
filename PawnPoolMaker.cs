using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PawnPoolMaker
{
	public static IEnumerable<Pawn> GenerateRaidPawns(PawnPoolRequest req)
	{
		List<EnemyGroup> poolGroupList = new List<EnemyGroup>();
		int pointsLeft = req.points;
		while (true)
		{
			List<EnemyGroup> possibleEnemies = (from sel in SelectableEnemies(req)
				where pointsLeft >= sel.cost
				select sel).ToList();
			if (possibleEnemies.Count == 0)
			{
				break;
			}
			EnemyGroup chosenGroup = possibleEnemies.RandomElementByWeight((EnemyGroup sel) => sel.weight);
			pointsLeft -= chosenGroup.cost;
			poolGroupList.Add(chosenGroup);
		}
		if (poolGroupList.Count == 0)
		{
			Debug.LogWarning("Tried to generate with only " + req.points + " points and got no pawns. Defaulting to a single drifter.");
			yield return PawnMaker.GeneratePawn("Drifter", TeamType.Raider);
			yield break;
		}
		foreach (EnemyGroup group in poolGroupList)
		{
			foreach (string name in group.pawnKindNames)
			{
				yield return PawnMaker.GeneratePawn(name, TeamType.Raider);
			}
		}
	}

	private static IEnumerable<EnemyGroup> SelectableEnemies(PawnPoolRequest req)
	{
		yield return new EnemyGroup(10, 25)
		{
			pawnKindNames = { "Drifter" }
		};
		yield return new EnemyGroup(40, 200)
		{
			pawnKindNames = { "Drifter", "Drifter", "Drifter", "Drifter", "Drifter", "Drifter", "Drifter", "Drifter" }
		};
		yield return new EnemyGroup(100, 110)
		{
			pawnKindNames = { "Scavenger", "Scavenger", "Scavenger" }
		};
		yield return new EnemyGroup(100, 70)
		{
			pawnKindNames = { "Grenadier" }
		};
		yield return new EnemyGroup(150, 180)
		{
			pawnKindNames = { "Pirate", "Pirate", "Pirate" }
		};
		yield return new EnemyGroup(140, 200)
		{
			pawnKindNames = { "Sniper", "Sniper", "Sniper" }
		};
		yield return new EnemyGroup(250, 280)
		{
			pawnKindNames = { "Mercenary", "Mercenary", "Mercenary" }
		};
	}
}
