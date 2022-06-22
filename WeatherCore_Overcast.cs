public class WeatherCore_Overcast : WeatherCore
{
	public WeatherCore_Overcast()
	{
		screenSaturation = 1f;
		skyColorsNightEdge = SkyColors.NightOvercast;
		skyColorsNightMid = SkyColors.NightOvercast;
		skyColorsDusk = SkyColors.DuskOvercast;
		skyColorsDay = SkyColors.DayOvercast;
		SetupSkyTargets();
	}
}
