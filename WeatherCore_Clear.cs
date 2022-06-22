public class WeatherCore_Clear : WeatherCore
{
	public WeatherCore_Clear()
	{
		screenSaturation = 1.25f;
		skyColorsNightEdge = SkyColors.NightEdgeClear;
		skyColorsNightMid = SkyColors.NightMidClear;
		skyColorsDusk = SkyColors.DuskClear;
		skyColorsDay = SkyColors.DayClear;
		SetupSkyTargets();
	}
}
