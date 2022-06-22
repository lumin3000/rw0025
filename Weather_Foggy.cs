public class Weather_Foggy : Weather
{
	public Weather_Foggy()
	{
		baseLabel = "Fog";
		commonality = 2f;
		windIntensity = 0.5f;
		accuracyMultiplier = 0.5f;
		favorability = IncidentFavorability.Neutral;
		core = new WeatherCore_Overcast();
		overlays.Add(WeatherPartPool.GetInstanceOf(typeof(WeatherOverlay_Fog)));
	}
}
