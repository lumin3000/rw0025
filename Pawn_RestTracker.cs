public class Pawn_RestTracker : Saveable
{
	private Pawn pawn;

	private StatusLevel_Rest pieceRest;

	public StatusLevel_Rest Rest => pieceRest;

	public bool DoneResting
	{
		get
		{
			if (pieceRest.curLevel > 99f)
			{
				return true;
			}
			if (SkyManager.curSkyGlowPercent * 30f + pieceRest.curLevel > 99f)
			{
				return true;
			}
			return false;
		}
	}

	public Pawn_RestTracker(Pawn pawn)
	{
		this.pawn = pawn;
		pieceRest = new StatusLevel_Rest(pawn);
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref pieceRest, "PieceRest", pawn);
	}

	public void RestTick()
	{
		pieceRest.StatusLevelTick();
	}
}
