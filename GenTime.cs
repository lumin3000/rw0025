using System;

public static class GenTime
{
	public static float TicksInSeconds(this int numTicks)
	{
		return (float)numTicks / 60f;
	}

	public static int SecondsInTicks(this float numSeconds)
	{
		return (int)Math.Round(60f * numSeconds);
	}

	public static string TicksInSecondsString(this int numTicks)
	{
		return numTicks.TicksInSeconds().ToString("######0.0") + " seconds";
	}

	public static string SecondsInTicksString(this float numSeconds)
	{
		return numSeconds.SecondsInTicks().ToString();
	}

	public static float TicksInDays(this int numTicks)
	{
		return (float)numTicks / 20000f;
	}

	public static string TicksInDaysString(this int numTicks)
	{
		return numTicks.TicksInDays().ToString("#####0.0 days");
	}
}
