using UnityEngine;

public class PawnFootprintMaker
{
	private const float FootprintIntervalDist = 0.4f;

	private const float LeftRightOffsetDist = 0.17f;

	private Pawn pawn;

	private Vector3 lastFootprintPlacePos;

	private bool lastFootprintRight;

	private static readonly Vector3 FootprintOffset = new Vector3(0f, 0f, -0.3f);

	public PawnFootprintMaker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void FootprintMakerTick()
	{
		if (pawn.raceDef.MakesFootprints && (pawn.drawer.tweener.TweenedPos - lastFootprintPlacePos).MagnitudeHorizontalSquared() > 0.4f)
		{
			PlaceFootprint();
		}
	}

	private void PlaceFootprint()
	{
		if (Find.TerrainGrid.TerrainAt(pawn.Position).takeFootprints)
		{
			Vector3 tweenedPos = pawn.drawer.tweener.TweenedPos;
			Vector3 normalized = (tweenedPos - lastFootprintPlacePos).normalized;
			float rot = normalized.AngleFlat();
			float angle = ((!lastFootprintRight) ? (-90) : 90);
			Vector3 vector = normalized.RotatedBy(angle) * 0.17f;
			Vector3 spawnLoc = tweenedPos + FootprintOffset + vector;
			MoteMaker.PlaceFootprint(spawnLoc, rot);
			lastFootprintPlacePos = tweenedPos;
			lastFootprintRight = !lastFootprintRight;
		}
	}
}
