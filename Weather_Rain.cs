public class Weather_Rain : Weather
{
	public Weather_Rain()
	{
		baseLabel = "Rain";
		commonality = 2f;
		windIntensity = 1.5f;
		accuracyMultiplier = 0.8f;
		favorability = IncidentFavorability.Neutral;
		core = new WeatherCore_Overcast();
		rainRate = 1f;
		moveTicksAddonPct = 0.2f;
		overlays.Add(WeatherPartPool.GetInstanceOf(typeof(WeatherOverlay_Rain)));
	}
}
