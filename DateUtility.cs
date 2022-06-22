using System.Text;
using UnityEngine;

public static class DateUtility
{
	public static int DaysPassedAt(int ticks)
	{
		return Mathf.FloorToInt((float)ticks / 20000f);
	}

	public static int DayOfCurrentCycleAt(int ticks)
	{
		return DaysPassedAt(ticks) % 10 + 1;
	}

	public static int CyclesPassedAt(int ticks)
	{
		return Mathf.FloorToInt((float)ticks / 200000f);
	}

	public static string AsDate(this int ticks)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("cycle ");
		stringBuilder.Append((CyclesPassedAt(ticks) + 1).ToString());
		stringBuilder.Append(" , day ");
		stringBuilder.Append(DayOfCurrentCycleAt(ticks));
		return stringBuilder.ToString();
	}

	public static string AsDatePeriod(this int ticks)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append((CyclesPassedAt(ticks) + 1).ToString());
		stringBuilder.Append(" cycles, ");
		stringBuilder.Append(DayOfCurrentCycleAt(ticks));
		stringBuilder.Append(" days");
		return stringBuilder.ToString();
	}
}
