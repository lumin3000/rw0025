using System.Collections.Generic;

public class WeatherEventHandler
{
	private List<WeatherEvent> activeEvents = new List<WeatherEvent>();

	public WeatherEvent OverridingWeatherEvent
	{
		get
		{
			foreach (WeatherEvent activeEvent in activeEvents)
			{
				if (activeEvent.OverrideSkyTarget != null)
				{
					return activeEvent;
				}
			}
			return null;
		}
	}

	public void AddEvent(WeatherEvent newEvent)
	{
		activeEvents.Add(newEvent);
		newEvent.FireEvent();
	}

	public void WeatherEventHandlerTick()
	{
		foreach (WeatherEvent activeEvent in activeEvents)
		{
			activeEvent.WeatherEventTick();
		}
		activeEvents.RemoveAll((WeatherEvent ev) => ev.Expired);
	}

	public void WeatherEventsDraw()
	{
		foreach (WeatherEvent activeEvent in activeEvents)
		{
			activeEvent.WeatherEventDraw();
		}
	}
}
