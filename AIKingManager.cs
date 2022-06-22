using System.Collections.Generic;

public class AIKingManager : Saveable
{
	public List<AIKing> allKings = new List<AIKing>();

	public void ExposeData()
	{
		Scribe.LookList(ref allKings, "KingList");
	}

	public void AIKingManagerTick()
	{
		foreach (AIKing allKing in allKings)
		{
			allKing.AIKingTick();
		}
	}

	public void AddKing(AIKing newKing)
	{
		allKings.Add(newKing);
	}

	public void RemoveKing(AIKing oldKing)
	{
		allKings.Remove(oldKing);
	}

	public AIKing KingOf(Pawn p)
	{
		foreach (AIKing allKing in allKings)
		{
			foreach (Pawn ownedPawn in allKing.ownedPawns)
			{
				if (ownedPawn == p)
				{
					return allKing;
				}
			}
		}
		return null;
	}
}
