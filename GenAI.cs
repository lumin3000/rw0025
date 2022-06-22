using System;
using System.Collections.Generic;
using System.Linq;

public static class GenAI
{
	public struct AIDestination
	{
		public IntVec3 pos;

		public bool found;

		public static AIDestination NotFound
		{
			get
			{
				AIDestination result = default(AIDestination);
				result.found = false;
				return result;
			}
		}

		public AIDestination(IntVec3 newPos)
		{
			pos = newPos;
			found = true;
		}
	}

	public static Thing ClosestReachableEnemyTarget(this Thing searcher, GenScan.CloseToThingValidator validator, float maxDistance, bool needsLOStoDynamic, bool needsLOStoStatic)
	{
		GenScan.CloseToThingValidator validator2 = delegate(Thing t)
		{
			if (validator != null && !validator(t))
			{
				return false;
			}
			if (searcher is Pawn && !searcher.CanReach(t, adjacentIsOK: true))
			{
				return false;
			}
			if ((needsLOStoDynamic || needsLOStoStatic) && !searcher.CanSee(t))
			{
				if (t is Pawn)
				{
					if (needsLOStoDynamic)
					{
						return false;
					}
				}
				else if (needsLOStoStatic)
				{
					return false;
				}
			}
			Pawn pawn2 = t as Pawn;
			return (pawn2 == null || !pawn2.Incapacitated) ? true : false;
		};
		AIKing aIKing = null;
		Pawn pawn = searcher as Pawn;
		if (pawn != null)
		{
			aIKing = pawn.GetKing();
		}
		return GenScan.ClosestThing(searchEnum: (aIKing == null) ? PawnTargetsFor(searcher.Team) : aIKing.hitList.GetHitListEnumerable(), searchCenter: searcher.Position, maxDistance: maxDistance, validator: validator2);
	}

	public static IEnumerable<Thing> PawnTargetsFor(TeamType Team)
	{
		return Find.PawnManager.PawnsWithHostilityTo[Team].Where((Pawn pawn) => !pawn.Incapacitated).Select((Func<Pawn, Thing>)((Pawn pawn) => pawn));
	}

	public static bool CanSee(this Thing Seer, Thing Other)
	{
		foreach (IntVec3 item in ShootLeanUtility.ShootingSourcesFromTo(Seer.Position, Other.Position))
		{
			foreach (IntVec3 item2 in ShootLeanUtility.ShootableSquaresOf(Other))
			{
				if (GenGrid.LineOfSight(item, item2))
				{
					return true;
				}
			}
		}
		return false;
	}
}
