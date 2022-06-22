using System;
using UnityEngine;

public class WeatherEventMaker
{
	public float averageTicksBetweenEvents = 100f;

	public Type eventClass;

	public void WeatherEventMakerTick(float strength)
	{
		if (UnityEngine.Random.value < 1f / averageTicksBetweenEvents * strength)
		{
			WeatherEvent newEvent = (WeatherEvent)Activator.CreateInstance(eventClass);
			Find.WeatherManager.eventHandler.AddEvent(newEvent);
		}
	}
}
