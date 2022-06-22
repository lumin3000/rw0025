using UnityEngine;

public class PawnTweener
{
	private const float SpringTightness = 0.09f;

	private Pawn pawn;

	private PawnFootprintMaker footprintMaker;

	private Vector3 springPos = new Vector3(0f, 0f, 0f);

	public Vector3 TweenedPos => springPos;

	public Vector3 TweenedPosRoot
	{
		get
		{
			float num = ((!pawn.pather.moving) ? 0f : ((pawn.pather.ThingBlockingNextPathSquare() != null) ? 0f : ((pawn.pather.NextSquareDoorToOpen() == null) ? (1f - (float)pawn.pather.ticksUntilMove / (float)pawn.pather.totalMoveDuration) : 0f)));
			if (pawn.stances.FullBodyBusy)
			{
				num = 0f;
			}
			return pawn.pather.nextSquare.ToVector3Shifted() * num + pawn.Position.ToVector3Shifted() * (1f - num);
		}
	}

	public PawnTweener(Pawn pawn)
	{
		this.pawn = pawn;
		footprintMaker = new PawnFootprintMaker(pawn);
	}

	public void TweenerTick()
	{
		Vector3 vector = TweenedPosRoot - springPos;
		springPos += vector * 0.09f;
		footprintMaker.FootprintMakerTick();
	}

	public void Notify_Spawned()
	{
		springPos = TweenedPosRoot;
	}

	public void Notify_Teleported_Int()
	{
		springPos = pawn.Position.ToVector3Shifted();
	}
}
