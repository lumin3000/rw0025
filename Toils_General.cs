using UnityEngine;

public static class Toils_General
{
	public static void GetToPickup(Pawn p, Thing target)
	{
		if (!p.CanReachForInteract(target))
		{
			p.jobs.EndCurrentJob(JobCondition.Incompletable);
		}
		else if (target.def.pathCost <= 0 && target.Position.Walkable())
		{
			p.pather.StartPathTowards(target.Position, autoReserve: false, adjacentIsOK: false);
		}
		else
		{
			p.pather.StartPathTowards(target);
		}
	}

	public static Toil ReserveTarget(Pawn pawn, Thing target, ReservationType rType)
	{
		return new Toil(delegate
		{
			if (!pawn.TryReserve(target, rType))
			{
				Debug.LogError(string.Concat(pawn, " could not reserve for interaction prisoner ", target));
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
		}, ToilCompleteMode.Immediate);
	}
}
