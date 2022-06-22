public class GameEnder : Saveable
{
	private const int GameEndCountdownDuration = 400;

	public bool gameEnding;

	private int ticksToGameOver = -1;

	public void ExposeData()
	{
		Scribe.LookField(ref gameEnding, "GameEnded");
		Scribe.LookField(ref ticksToGameOver, "TicksToGameOver", -1);
	}

	public void CheckGameOver()
	{
		if (Find.TickManager.tickCount < 300 || gameEnding)
		{
			return;
		}
		foreach (Pawn colonist in Find.PawnManager.Colonists)
		{
			if (!colonist.destroyed)
			{
				return;
			}
		}
		gameEnding = true;
		ticksToGameOver = 400;
	}

	public void GameEndTick()
	{
		if (gameEnding)
		{
			ticksToGameOver--;
			if (ticksToGameOver == 0)
			{
				GenGameEnd.GameOverEveryoneDead();
			}
		}
	}
}
