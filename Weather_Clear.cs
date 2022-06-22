public class Weather_Clear : Weather
{
	public Weather_Clear()
	{
		baseLabel = "Clear";
		commonality = 16f;
		repeatable = true;
		favorability = IncidentFavorability.Good;
		core = new WeatherCore_Clear();
	}
}
