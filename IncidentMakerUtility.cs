using UnityEngine;

public static class IncidentMakerUtility
{
	public static int TimeAdjustedThreatPointsNow
	{
		get
		{
			float num = Find.TickManager.tickCount / 100000;
			return 30 + Mathf.RoundToInt(num * 80f);
		}
	}

	public static int StrengthAdjustedThreatPointsNow
	{
		get
		{
			float strengthRating = Find.Storyteller.watcher.watcherStrength.StrengthRating;
			return Mathf.RoundToInt(40f * strengthRating);
		}
	}
}
