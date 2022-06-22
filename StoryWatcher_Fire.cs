public class StoryWatcher_Fire
{
	private const float DangerPerFire = 0.015f;

	private const float FireSizeFactor = 0.01f;

	public float fireAmount;

	public bool LargeFireDangerPresent => fireAmount > 1.3f;

	public void UpdateObservations()
	{
		fireAmount = 0f;
		foreach (Fire spawnedFire in Find.ThingLister.spawnedFires)
		{
			fireAmount += 0.015f + 0.01f * spawnedFire.fireSize;
		}
	}
}
