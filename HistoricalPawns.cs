using System.Collections.Generic;

public class HistoricalPawns : Saveable
{
	private List<Pawn> allHistoricalPawns = new List<Pawn>();

	public void ExposeData()
	{
	}

	public void Notify_PawnDestroyed(Pawn pawn)
	{
	}
}
