using System.Collections.Generic;

public class Weather : Saveable
{
	public float commonality = 999f;

	public bool repeatable;

	public IncidentFavorability favorability = IncidentFavorability.Neutral;

	public float rainRate;

	public float accuracyMultiplier = 1f;

	public float windIntensity = 1f;

	public float moveTicksAddonPct;

	protected string baseLabel;

	public WeatherCore core;

	public List<WeatherOverlay> overlays = new List<WeatherOverlay>();

	public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

	public virtual string Label => baseLabel;

	public virtual void DrawWeather()
	{
		foreach (WeatherOverlay overlay in overlays)
		{
			overlay.DrawWeatherOverlay();
		}
	}

	public virtual void WeatherTick(float strength)
	{
		foreach (WeatherOverlay overlay in overlays)
		{
			overlay.WeatherOverlayTick();
		}
		foreach (WeatherEventMaker eventMaker in eventMakers)
		{
			eventMaker.WeatherEventMakerTick(strength);
		}
	}

	public virtual void ExposeData()
	{
	}
}
