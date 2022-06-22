using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeatherDecider : Saveable
{
	public const int MinWeatherDuration = 3000;

	private int lastWeatherStartTick;

	private float weatherChangeChangeEveryHundredTicks = 0.01f;

	private int CurWeatherAge => Find.TickManager.tickCount - lastWeatherStartTick;

	public static IEnumerable<Weather> AllWeathers => from type in typeof(Weather).AllSubclasses()
		select (Weather)Activator.CreateInstance(type);

	public void ExposeData()
	{
		Scribe.LookField(ref lastWeatherStartTick, "LastWeatherStartTick", forceSave: true);
	}

	public void WeatherDeciderTick()
	{
		if (CurWeatherAge > 3000)
		{
			float num = weatherChangeChangeEveryHundredTicks / 100f;
			if (Find.Storyteller.watcher.watcherFire.LargeFireDangerPresent)
			{
				num *= 10f;
			}
			if (UnityEngine.Random.value < num)
			{
				StartNextWeather();
			}
		}
	}

	private void StartNextWeather()
	{
		Weather newWeather = Find.Storyteller.NextWeather();
		Find.WeatherManager.TransitionTo(newWeather);
		lastWeatherStartTick = Find.TickManager.tickCount;
	}
}
