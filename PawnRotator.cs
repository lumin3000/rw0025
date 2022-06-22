public class PawnRotator
{
	private Pawn pawn;

	public PawnRotator(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void PawnRotatorTick()
	{
		if (pawn.pather.moving)
		{
			if (pawn.pather.curPath != null && pawn.pather.curPath.nodeList.Count >= 1)
			{
				FaceAdjacentSquare(pawn.pather.nextSquare);
			}
			return;
		}
		Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
		if (stance_Busy != null)
		{
			if (stance_Busy.focusTarg != null)
			{
				FaceSquare(stance_Busy.focusTarg.Loc);
			}
			return;
		}
		if (pawn.jobs.CurJob != null && pawn.jobs.CurJob.targetA != null)
		{
			TargetPack targetA = pawn.jobs.CurJob.targetA;
			if (targetA.HasThing)
			{
				bool flag = false;
				IntVec3 sq = default(IntVec3);
				foreach (IntVec3 item in Gen.SquaresOccupiedBy(targetA.thing))
				{
					if (item.AdjacentToCardinal(pawn.Position))
					{
						FaceAdjacentSquare(item);
						return;
					}
					if (item.AdjacentTo8Way(pawn.Position))
					{
						flag = true;
						sq = item;
					}
				}
				if (flag)
				{
					if (DebugSettings.drawPawnRotatorTarget)
					{
						Find.DebugDrawer.MakeDebugSquare(pawn.Position, "jbthing", 60, 100);
						GenRender.DrawLineBetween(pawn.Position.ToVector3Shifted(), sq.ToVector3Shifted());
					}
					FaceAdjacentSquare(sq);
					return;
				}
			}
			else if (pawn.Position.AdjacentTo8Way(targetA.Loc))
			{
				if (DebugSettings.drawPawnRotatorTarget)
				{
					Find.DebugDrawer.MakeDebugSquare(pawn.Position, "jbloc", 20, 100);
					GenRender.DrawLineBetween(pawn.Position.ToVector3Shifted(), targetA.Loc.ToVector3Shifted());
				}
				FaceAdjacentSquare(targetA.Loc);
				return;
			}
		}
		if (pawn.raceDef.humanoid)
		{
			pawn.rotation = IntRot.south;
		}
	}

	private void FaceAdjacentSquare(IntVec3 sq)
	{
		IntVec3 intVec = sq - pawn.Position;
		if (intVec.x > 0)
		{
			pawn.rotation = IntRot.east;
		}
		else if (intVec.x < 0)
		{
			pawn.rotation = IntRot.west;
		}
		else if (intVec.z > 0)
		{
			pawn.rotation = IntRot.north;
		}
		else
		{
			pawn.rotation = IntRot.south;
		}
	}

	public void FaceSquare(IntVec3 sq)
	{
		float angle = (sq - pawn.Position).ToVector3().AngleFlat();
		pawn.rotation = RotFromAngleBiased(angle);
	}

	private IntRot RotFromAngleBiased(float angle)
	{
		if (angle < 30f)
		{
			return IntRot.north;
		}
		if (angle < 150f)
		{
			return IntRot.east;
		}
		if (angle < 210f)
		{
			return IntRot.south;
		}
		if (angle < 330f)
		{
			return IntRot.west;
		}
		return IntRot.north;
	}
}
