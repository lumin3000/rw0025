using System;
using UnityEngine;

public static class CastPositionFinder
{
	private const float OpenGroundPreference = 0.3f;

	private const float MinimumPreferredRange = 5f;

	private const float OptimalRangeFactor = 0.8f;

	private const float OptimalRangeFactorImportance = 0.3f;

	private static CastingPositionRequest req;

	private static IntVec3 moverLoc;

	private static IntVec3 targetLoc;

	private static float rangeFromTargetSquared;

	private static Verb primaryAttack;

	private static float optimalRangeSquared;

	private static float rangeFromMoverToSquareSquared;

	private static float rangeFromTargetToSquareSquared;

	public static GenAI.AIDestination FindCastingPosition(CastingPositionRequest newReq)
	{
		req = newReq;
		if (!req.moverPawn.CanReach(req.targetThing, adjacentIsOK: true))
		{
			Debug.LogError(string.Concat(req.moverPawn, " cannot find casting positing for unreachable target ", req.targetThing));
			return GenAI.AIDestination.NotFound;
		}
		moverLoc = req.moverPawn.Position;
		targetLoc = req.targetThing.Position;
		primaryAttack = req.moverPawn.equipment.Primary.verb;
		if (primaryAttack == null)
		{
			Debug.LogWarning(string.Concat(req.moverPawn, " trying to find casting position without primary attack."));
			return GenAI.AIDestination.NotFound;
		}
		IntRect wholeMap = IntRect.WholeMap;
		if (req.maxRangeFromMover < 5000f)
		{
			int num = Mathf.CeilToInt(req.maxRangeFromMover);
			IntRect otherRect = new IntRect(moverLoc.x - num, moverLoc.z - num, num * 2 + 1, num * 2 + 1);
			wholeMap.ClipInsideRect(otherRect);
		}
		int num2 = Mathf.CeilToInt(req.maxRangeFromTarget);
		IntRect otherRect2 = new IntRect(targetLoc.x - num2, targetLoc.z - num2, num2 * 2 + 1, num2 * 2 + 1);
		wholeMap.ClipInsideRect(otherRect2);
		if (req.maxRangeFromDefendHome < 5000f)
		{
			int num3 = Mathf.CeilToInt(req.maxRangeFromDefendHome);
			IntRect intRect = new IntRect(targetLoc.x - num3, targetLoc.z - num3, num3 * 2 + 1, num3 * 2 + 1);
			wholeMap.ClipInsideRect(otherRect2);
		}
		IntVec3 newPos = moverLoc;
		float num4 = 0.001f;
		bool flag = false;
		float num5 = req.maxRangeFromMover * req.maxRangeFromMover;
		float num6 = req.maxRangeFromTarget * req.maxRangeFromTarget;
		float num7 = req.maxRangeFromDefendHome * req.maxRangeFromDefendHome;
		rangeFromTargetSquared = (req.moverPawn.Position - req.targetThing.Position).LengthHorizontalSquared;
		optimalRangeSquared = primaryAttack.VerbDef.range * 0.8f * (primaryAttack.VerbDef.range * 0.8f);
		foreach (IntVec3 item in wholeMap)
		{
			rangeFromMoverToSquareSquared = (item - req.moverPawn.Position).LengthHorizontalSquared;
			if (rangeFromMoverToSquareSquared > num5 || (req.maxRangeFromDefendHome < 5000f && (item - req.defendHome).LengthHorizontalSquared > num7))
			{
				continue;
			}
			rangeFromTargetToSquareSquared = (item - req.targetThing.Position).LengthHorizontalSquared;
			if (!(rangeFromTargetToSquareSquared > num6) && item.Standable() && primaryAttack.CanHitTargetFrom(item, new TargetPack(req.targetThing)) && !Find.PawnDestinationManager.DestinationIsReserved(item, req.moverPawn))
			{
				float num8 = AIPreferenceAtFrom(item);
				if (num8 > num4)
				{
					flag = true;
					newPos = item;
					num4 = num8;
				}
				if (Debug.isDebugBuild && DebugSettings.drawCastPositionSearch)
				{
					Find.DebugDrawer.MakeDebugSquare(item, ((int)(num8 * 100f)).ToString(), (int)(num8 * 100f), 100);
				}
			}
		}
		if (flag)
		{
			return new GenAI.AIDestination(newPos);
		}
		return GenAI.AIDestination.NotFound;
	}

	private static float AIPreferenceAtFrom(IntVec3 sq)
	{
		if (Find.Grids.SquareContains(sq, EntityType.Fire))
		{
			return -1f;
		}
		if (req.targetThing.Position == sq || req.targetThing.Position.AdjacentTo8Way(sq))
		{
			return 0f;
		}
		float num = 0.3f;
		if (req.moverPawn.kindDef.aiAvoidCover)
		{
			num += 8f - CoverUtility.SimpleCoverScoreAt(sq);
		}
		if (req.wantCoverFromTarget)
		{
			num += CoverUtility.CoverGiverSetAtFrom(sq, req.targetThing.Position).overallBlockChance;
		}
		float lengthHorizontalFast = (sq - req.moverPawn.Position).LengthHorizontalFast;
		float num2 = (float)Math.Pow(0.9800000190734863, lengthHorizontalFast);
		num *= num2;
		if (rangeFromTargetToSquareSquared < 25f)
		{
			num *= 0.5f;
		}
		if (rangeFromMoverToSquareSquared > rangeFromTargetSquared)
		{
			num *= 0.4f;
		}
		float num3 = Math.Abs(rangeFromTargetToSquareSquared - optimalRangeSquared) / optimalRangeSquared;
		num3 = 1f - num3;
		num3 = 0.7f + 0.3f * num3;
		return num * num3;
	}
}
