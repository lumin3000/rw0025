public class Weather_LightningStormRainy : Weather
{
	public Weather_LightningStormRainy()
	{
		baseLabel = "Rainy thunderstorm";
		commonality = 2f;
		windIntensity = 1.5f;
		accuracyMultiplier = 0.8f;
		favorability = IncidentFavorability.Bad;
		core = new WeatherCore_Overcast();
		rainRate = 1f;
		moveTicksAddonPct = 0.2f;
		overlays.Add(WeatherPartPool.GetInstanceOf(typeof(WeatherOverlay_Rain)));
		WeatherEventMaker item = new WeatherEventMaker
		{
			averageTicksBetweenEvents = 400f,
			eventClass = typeof(WeatherEvent_Lightning)
		};
		eventMakers.Add(item);
		WeatherEventMaker item2 = new WeatherEventMaker
		{
			averageTicksBetweenEvents = 1200f,
			eventClass = typeof(WeatherEvent_LightningStrike)
		};
		eventMakers.Add(item2);
	}
}
