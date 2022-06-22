using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SkyManager
{
	public const float NightLength = 0.12f;

	private const float ShadowMaxLengthDay = 15f;

	private const float ShadowMaxLengthNight = 6f;

	public static float curSkyGlowPercent;

	private static readonly Color FogOfWarBaseColor = new Color32(63, 51, 46, byte.MaxValue);

	public static void UpdateSkylight()
	{
		SkyTarget skyTarget = CurrentWeatherSkytarget();
		float curDayPercent = DateHandler.CurDayPercent;
		if (skyTarget.overrideShadowVector)
		{
			SetSunShadowVector(skyTarget.shadowVector);
		}
		else
		{
			float num;
			float y;
			if (curDayPercent < 0.88f && curDayPercent > 0.12f)
			{
				num = Mathf.Lerp(15f, -15f, curDayPercent);
				y = -1.5f - 2.5f * (num * num / 100f);
			}
			else
			{
				float t = ((!(curDayPercent > 0.88f)) ? (0.5f + curDayPercent / 0.12f * 0.5f) : ((curDayPercent - 0.88f) / 0.12f * 0.5f));
				num = Mathf.Lerp(6f, -6f, t);
				y = -0.9f - 2.5f * (num * num / 100f);
			}
			SetSunShadowVector(new Vector2(num, y));
		}
		curSkyGlowPercent = skyTarget.glowPercent;
		MatBases.LightOverlay.color = skyTarget.colors.sky;
		Color sky = skyTarget.colors.sky;
		sky.a = 1f;
		sky *= FogOfWarBaseColor;
		MatBases.FogOfWar.color = sky;
		MatBases.SunShadow.color = skyTarget.colors.shadow;
		Find.CameraColor.saturation = skyTarget.saturation;
		Color weatherOverlays = skyTarget.colors.weatherOverlays;
		Weather lastWeather = Find.WeatherManager.lastWeather;
		Weather curWeather = Find.WeatherManager.curWeather;
		List<WeatherOverlay> list = lastWeather.overlays.Concat(curWeather.overlays).Distinct().ToList();
		foreach (WeatherOverlay item in list)
		{
			float a = 0.5f;
			if (lastWeather.overlays.Contains(item) && curWeather.overlays.Contains(item))
			{
				a = 1f;
			}
			else if (curWeather.overlays.Contains(item))
			{
				a = Find.WeatherManager.TransitionLerpFactor;
			}
			else if (lastWeather.overlays.Contains(item))
			{
				a = 1f - Find.WeatherManager.TransitionLerpFactor;
			}
			item.OverlayColor = new Color(weatherOverlays.r, weatherOverlays.g, weatherOverlays.b, a);
		}
	}

	public static void SetSunShadowVector(Vector2 vec)
	{
		Vector4 vector = new Vector4(vec.x, 0f, vec.y, 0f);
		MatBases.SunShadow.SetVector("_CastVect", vector);
	}

	private static SkyTarget CurrentWeatherSkytarget()
	{
		SkyTarget b = LerpedSkyTargetFrom(Find.WeatherManager.curWeather, DateHandler.CurDayPercent);
		SkyTarget a = LerpedSkyTargetFrom(Find.WeatherManager.lastWeather, DateHandler.CurDayPercent);
		SkyTarget skyTarget = SkyTarget.Lerp(a, b, Find.WeatherManager.TransitionLerpFactor);
		if (Find.MapConditionManager.WeatherLerpFactor > 0f)
		{
			skyTarget = SkyTarget.Lerp(skyTarget, Find.MapConditionManager.WeatherLerpTarget, Find.MapConditionManager.WeatherLerpFactor);
		}
		WeatherEvent overridingWeatherEvent = Find.WeatherManager.eventHandler.OverridingWeatherEvent;
		if (overridingWeatherEvent != null)
		{
			skyTarget = SkyTarget.Lerp(skyTarget, overridingWeatherEvent.OverrideSkyTarget, overridingWeatherEvent.OverrideSkyTargetLerpFactor);
		}
		return skyTarget;
	}

	private static SkyTarget LerpedSkyTargetFrom(Weather curWeather, float curDayPercent)
	{
		List<SkyTarget> skyTargets = curWeather.core.skyTargets;
		SkyTarget skyTarget = skyTargets.Where((SkyTarget t) => t.dayPercent < curDayPercent).LastOrDefault();
		if (skyTarget == null)
		{
			skyTarget = skyTargets[0];
		}
		SkyTarget skyTarget2 = skyTargets.Where((SkyTarget t) => t.dayPercent >= curDayPercent).FirstOrDefault();
		if (skyTarget2 == null)
		{
			skyTarget2 = skyTargets[0];
		}
		float factor = (curDayPercent - skyTarget.dayPercent) / (skyTarget2.dayPercent - skyTarget.dayPercent);
		return SkyTarget.Lerp(skyTarget, skyTarget2, factor);
	}
}
