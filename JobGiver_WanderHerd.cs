using UnityEngine;

public class JobGiver_WanderHerd : JobGiver_Wander
{
	private const int MinDistToHumanoid = 15;

	public JobGiver_WanderHerd()
	{
		radius = 5f;
		ticksBetweenWandersRange = new IntRange(125, 200);
	}

	public override void SetPawn(Pawn newPawn)
	{
		base.SetPawn(newPawn);
		wanderDestValidator = (IntVec3 loc) => WanderUtility.InSameRoom(newPawn.Position, loc);
	}

	protected override IntVec3 GetWanderRoot()
	{
		GenScan.CloseToThingValidator validator = delegate(Thing t)
		{
			if (((Pawn)t).raceDef != pawn.raceDef || t == pawn)
			{
				return false;
			}
			if (!WanderUtility.InSameRoom(pawn.Position, t.Position))
			{
				return false;
			}
			if (Random.value < 0.5f)
			{
				return false;
			}
			foreach (Pawn allPawn in Find.PawnManager.AllPawns)
			{
				if (allPawn.raceDef.humanoid && (allPawn.Position - t.Position).LengthHorizontalSquared < 225f)
				{
					return false;
				}
			}
			return true;
		};
		return GenScan.ClosestReachableThing(pawn.Position, Find.PawnManager.AllPawns, 35f, validator)?.Position ?? pawn.Position;
	}
}
