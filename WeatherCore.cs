using System.Collections.Generic;

public class WeatherCore
{
	protected float screenSaturation = 1f;

	protected SkyColorSet skyColorsNightMid;

	protected SkyColorSet skyColorsNightEdge;

	protected SkyColorSet skyColorsDay;

	protected SkyColorSet skyColorsDusk;

	public List<SkyTarget> skyTargets = new List<SkyTarget>();

	protected void SetupSkyTargets()
	{
		float num = 0.12f;
		skyTargets.Add(new SkyTarget(0f, 0f, skyColorsNightMid));
		skyTargets.Add(new SkyTarget(0.06f, 0f, skyColorsNightMid));
		skyTargets.Add(new SkyTarget(num, 0f, skyColorsNightEdge));
		skyTargets.Add(new SkyTarget(0.2f, 0.5f, skyColorsDusk));
		skyTargets.Add(new SkyTarget(0.3f, 1f, skyColorsDay));
		skyTargets.Add(new SkyTarget(0.7f, 1f, skyColorsDay));
		skyTargets.Add(new SkyTarget(0.8f, 0.5f, skyColorsDusk));
		skyTargets.Add(new SkyTarget(1f - num, 0f, skyColorsNightEdge));
		skyTargets.Add(new SkyTarget(0.94f, 0f, skyColorsNightMid));
		foreach (SkyTarget skyTarget in skyTargets)
		{
			skyTarget.colors.sky.a = skyTarget.glowPercent;
			skyTarget.saturation = screenSaturation;
		}
	}
}
