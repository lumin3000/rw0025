public static class DateHandler
{
	public const int TicksPerDay = 20000;

	public const int DaysPerCycle = 10;

	public const int TicksPerCycle = 200000;

	public const float SecondsPerTickAsFractionOfDay = 4f;

	private static int TickCount => Find.TickManager.tickCount;

	public static int DaysPassed => DateUtility.DaysPassedAt(TickCount);

	public static int CyclesPassed => DateUtility.CyclesPassedAt(TickCount);

	public static int DayOfCurrentCycle => DateUtility.DayOfCurrentCycleAt(TickCount);

	public static float CurDayPercent
	{
		get
		{
			int num = Find.TickManager.tickCount % 20000;
			if (num == 0)
			{
				num = 1;
			}
			return (float)num / 20000f;
		}
	}

	public static void DateHandlerTick()
	{
		if (Find.TickManager.tickCount % 20000 == 0)
		{
			PassDay();
		}
	}

	private static void PassDay()
	{
		Find.ResourceManager.DayPassed();
	}
}
