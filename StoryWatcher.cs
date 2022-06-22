using System.Text;

public class StoryWatcher
{
	private const int TickMod = 15612;

	private const int CheckInterval = 426;

	public StoryWatcher_Fire watcherFire = new StoryWatcher_Fire();

	public StoryWatcher_Strength watcherStrength = new StoryWatcher_Strength();

	public string DebugReadout
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Watcher: ");
			stringBuilder.AppendLine("  fireAmount: " + watcherFire.fireAmount.ToString("####0.00"));
			if (watcherFire.LargeFireDangerPresent)
			{
				stringBuilder.AppendLine("    Fire danger present");
			}
			stringBuilder.AppendLine("  strength: " + watcherStrength.StrengthRating.ToString("####0.00"));
			return stringBuilder.ToString();
		}
	}

	public void StoryWatcherTick()
	{
		if ((Find.TickManager.tickCount + 15612) % 426 == 0)
		{
			watcherFire.UpdateObservations();
		}
	}
}
