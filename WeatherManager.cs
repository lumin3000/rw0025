using UnityEngine;

public class WeatherManager : Saveable
{
	public const float TransitionTicks = 1000f;

	public WeatherEventHandler eventHandler = new WeatherEventHandler();

	public Weather curWeather = new Weather_Clear();

	public Weather lastWeather = new Weather_Clear();

	public int ticksSinceTransition;

	public float TransitionLerpFactor
	{
		get
		{
			float num = (float)ticksSinceTransition / 1000f;
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}
	}

	public float RainRate => Mathf.Lerp(lastWeather.rainRate, curWeather.rainRate, TransitionLerpFactor);

	public float CurMoveTicksAddon => Mathf.Lerp(lastWeather.moveTicksAddonPct, curWeather.moveTicksAddonPct, TransitionLerpFactor);

	public float CurWindIntensity => Mathf.Lerp(lastWeather.windIntensity, curWeather.windIntensity, TransitionLerpFactor);

	public float CurWeatherAccuracyMultiplier => Mathf.Lerp(lastWeather.accuracyMultiplier, curWeather.accuracyMultiplier, TransitionLerpFactor);

	public void ExposeData()
	{
		Scribe.LookSaveable(ref curWeather, "CurWeather");
		Scribe.LookSaveable(ref lastWeather, "LastWeather");
		Scribe.LookField(ref ticksSinceTransition, "ticksSinceTransition", forceSave: true);
	}

	public void TransitionTo(Weather newWeather)
	{
		lastWeather = curWeather;
		curWeather = newWeather;
		ticksSinceTransition = 0;
	}

	public void DoWeatherGUI(Rect rect)
	{
		GUI.skin.label.alignment = TextAnchor.MiddleRight;
		Rect position = new Rect(rect);
		position.width -= 15f;
		GenUI.SetFontSmall();
		GUI.Label(position, curWeather.Label);
		TooltipHandler.TipRegion(rect, "The current weather.");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}

	public void WeatherManagerTick()
	{
		eventHandler.WeatherEventHandlerTick();
		ticksSinceTransition++;
		curWeather.WeatherTick(TransitionLerpFactor);
		lastWeather.WeatherTick(1f - TransitionLerpFactor);
	}

	public void DrawAllWeather()
	{
		eventHandler.WeatherEventsDraw();
		lastWeather.DrawWeather();
		curWeather.DrawWeather();
	}
}
