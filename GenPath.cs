using System;

public static class GenPath
{
	public static bool IsGoodWorkSpotFor(this IntVec3 sq, Pawn pawn)
	{
		return sq.Standable() && !Find.PawnDestinationManager.DestinationIsReserved(sq, pawn) && !sq.IsBurningImmobile();
	}

	private static PawnPath PathFromTo(Pawn pathingPawn, IntVec3 A, TargetPack B)
	{
		PathRequest pathRequest = new PathRequest();
		pathRequest.start = A;
		pathRequest.dest = B;
		pathRequest.pathParams = PathParameters.smart;
		pathRequest.pawn = pathingPawn;
		return PathSolver.FindPath(pathRequest);
	}

	public static bool CanGetAdjacentTo(this Pawn pawn, Thing targetThing)
	{
		foreach (IntVec3 item in Gen.AdjacentSquares8Way(targetThing))
		{
			if (item.IsGoodWorkSpotFor(pawn))
			{
				return true;
			}
		}
		return false;
	}

	public static IntVec3 SpotToStandAdjacentToFor(Pawn pawn, Thing targetThing, out bool succeeded)
	{
		if (pawn.Position.AdjacentTo8Way(targetThing) && !Find.PawnDestinationManager.DestinationIsReserved(pawn.Position, pawn))
		{
			succeeded = true;
			return pawn.Position;
		}
		PawnPath pawnPath = PathFromTo(pawn, pawn.Position, targetThing);
		if (!pawnPath.found)
		{
			succeeded = false;
			return IntVec3.Invalid;
		}
		if (!Find.PawnDestinationManager.DestinationIsReserved(pawnPath.LastNode, pawn))
		{
			succeeded = true;
			return pawnPath.LastNode;
		}
		IntVec3 lastNode = pawnPath.LastNode;
		IntVec3 intVec = pawnPath.LastNode;
		bool flag = false;
		int num = Gen.NumSquaresInRadius(Math.Max(targetThing.def.size.x + 2, targetThing.def.size.z + 2));
		for (int i = 0; i < num; i++)
		{
			intVec = lastNode + Gen.RadialPattern[i];
			if (intVec.AdjacentTo8Way(targetThing) && intVec.IsGoodWorkSpotFor(pawn))
			{
				PawnPath pawnPath2 = PathFromTo(pawn, lastNode, new TargetPack(intVec));
				if (pawnPath2.found && pawnPath2.cost < 500f)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (flag)
		{
			succeeded = true;
			return intVec;
		}
		succeeded = false;
		return IntVec3.Invalid;
	}
}
