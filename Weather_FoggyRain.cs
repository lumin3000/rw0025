public class Weather_FoggyRain : Weather
{
	public Weather_FoggyRain()
	{
		commonality = 2f;
		baseLabel = "Foggy rain";
		windIntensity = 1f;
		accuracyMultiplier = 0.5f;
		favorability = IncidentFavorability.Neutral;
		core = new WeatherCore_Overcast();
		rainRate = 1f;
		moveTicksAddonPct = 0.2f;
		overlays.Add(WeatherPartPool.GetInstanceOf(typeof(WeatherOverlay_Fog)));
		overlays.Add(WeatherPartPool.GetInstanceOf(typeof(WeatherOverlay_Rain)));
	}
}
