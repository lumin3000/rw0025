using System.Collections.Generic;

public static class ShootLeanUtility
{
	public static IEnumerable<IntVec3> ShootingSourcesFromTo(IntVec3 ShooterPos, IntVec3 TargetPos)
	{
		float TargAng = (TargetPos - ShooterPos).AngleFlat;
		bool TargAbove = TargAng > 270f || TargAng < 90f;
		bool TargBelow = TargAng > 90f && TargAng < 270f;
		bool TargLeft = TargAng > 180f;
		bool TargRight = TargAng < 180f;
		yield return ShooterPos;
		bool[] Blocked = new bool[8];
		for (int j = 0; j < 8; j++)
		{
			Blocked[j] = !(ShooterPos + Gen.AdjacentSquares[j]).CanBeSeenOver();
		}
		if (!Blocked[1] && ((Blocked[0] && !Blocked[5] && TargAbove) || (Blocked[2] && !Blocked[4] && TargBelow)))
		{
			yield return ShooterPos + new IntVec3(1, 0, 0);
		}
		if (!Blocked[3] && ((Blocked[0] && !Blocked[6] && TargAbove) || (Blocked[2] && !Blocked[7] && TargBelow)))
		{
			yield return ShooterPos + new IntVec3(-1, 0, 0);
		}
		if (!Blocked[2] && ((Blocked[3] && !Blocked[7] && TargLeft) || (Blocked[1] && !Blocked[4] && TargRight)))
		{
			yield return ShooterPos + new IntVec3(0, 0, -1);
		}
		if (!Blocked[0] && ((Blocked[3] && !Blocked[6] && TargLeft) || (Blocked[1] && !Blocked[5] && TargRight)))
		{
			yield return ShooterPos + new IntVec3(0, 0, 1);
		}
		for (int i = 0; i < 4; i++)
		{
			if (!Blocked[i] && (i != 0 || TargAbove) && (i != 1 || TargRight) && (i != 2 || TargBelow) && (i != 3 || TargLeft))
			{
				CoverUtility.CoverGiver LeanGiver = CoverUtility.RawCoverGiverIn(ShooterPos + Gen.AdjacentSquares[i]);
				if (LeanGiver != null)
				{
					yield return ShooterPos + Gen.AdjacentSquares[i];
				}
			}
		}
	}

	public static IEnumerable<IntVec3> ShootableSquaresOf(Thing t)
	{
		if (t is Pawn)
		{
			yield return t.Position;
			foreach (IntVec3 Adj in t.Position.AdjacentSquaresCardinal())
			{
				if (Adj.CanBeSeenOver())
				{
					yield return Adj;
				}
			}
			yield break;
		}
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(t))
		{
			yield return item;
		}
	}
}
