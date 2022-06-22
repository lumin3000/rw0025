using UnityEngine;

public class PawnLeaner
{
	private const float leanOffsetPctChangeRate = 0.075f;

	private const float leanOffsetDistanceMultiplier = 0.5f;

	private Pawn pawn;

	private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

	private float leanOffsetCurPct;

	public Vector3 LeanOffset => shootSourceOffset.ToVector3() * 0.5f * leanOffsetCurPct;

	public PawnLeaner(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void LeanerTick()
	{
		if (ShouldLean())
		{
			leanOffsetCurPct += 0.075f;
			if (leanOffsetCurPct > 1f)
			{
				leanOffsetCurPct = 1f;
			}
		}
		else
		{
			leanOffsetCurPct -= 0.075f;
			if (leanOffsetCurPct < 0f)
			{
				leanOffsetCurPct = 0f;
			}
		}
	}

	private bool ShouldLean()
	{
		Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
		if (stance_Busy != null)
		{
			if (shootSourceOffset == new IntVec3(0, 0, 0))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
	{
		shootSourceOffset = newShootLine.source - pawn.Position;
	}
}
