public class Weather_LightningStormDry : Weather
{
	public Weather_LightningStormDry()
	{
		baseLabel = "Dry thunderstorm";
		commonality = 2f;
		windIntensity = 1.5f;
		favorability = IncidentFavorability.VeryBad;
		core = new WeatherCore_Overcast();
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
