using UnityEngine;

public static class SkyColors
{
	public static readonly SkyColorSet DayClear;

	public static readonly SkyColorSet DuskClear;

	public static readonly SkyColorSet NightEdgeClear;

	public static readonly SkyColorSet NightMidClear;

	public static readonly SkyColorSet DayOvercast;

	public static readonly SkyColorSet DuskOvercast;

	public static readonly SkyColorSet NightOvercast;

	public static readonly SkyColorSet LightningFlash;

	static SkyColors()
	{
		DayClear = default(SkyColorSet);
		DayClear.sky = Color.white;
		DayClear.shadow = new Color(61f / 85f, 38f / 51f, 193f / 255f);
		DayClear.weatherOverlays = new Color(1f, 1f, 1f);
		DuskClear = default(SkyColorSet);
		DuskClear.sky = new Color(73f / 85f, 166f / 255f, 36f / 85f);
		DuskClear.shadow = new Color(218f / 255f, 226f / 255f, 233f / 255f);
		DuskClear.weatherOverlays = new Color(0.8f, 0.8f, 0.8f);
		NightEdgeClear = default(SkyColorSet);
		NightEdgeClear.sky = new Color(0.482f, 0.603f, 0.682f);
		NightEdgeClear.shadow = Color.white;
		NightEdgeClear.weatherOverlays = new Color(0.6f, 0.6f, 0.6f);
		NightMidClear = default(SkyColorSet);
		NightMidClear.sky = new Color(0.482f, 0.603f, 0.682f);
		NightMidClear.shadow = new Color(0.85f, 0.85f, 0.85f);
		NightMidClear.weatherOverlays = new Color(0.6f, 0.6f, 0.6f);
		DayOvercast = default(SkyColorSet);
		DayOvercast.sky = new Color(0.8f, 0.8f, 0.8f);
		DayOvercast.shadow = Color.white;
		DayOvercast.weatherOverlays = new Color(0.7f, 0.7f, 0.7f);
		DuskOvercast = default(SkyColorSet);
		DuskOvercast.sky = new Color(0.6f, 0.6f, 0.6f);
		DuskOvercast.shadow = Color.white;
		DuskOvercast.weatherOverlays = new Color(0.6f, 0.6f, 0.6f);
		NightOvercast = default(SkyColorSet);
		NightOvercast.sky = new Color(0.35f, 0.4f, 0.45f);
		NightOvercast.shadow = Color.white;
		NightOvercast.weatherOverlays = new Color(0.5f, 0.5f, 0.5f);
		LightningFlash = default(SkyColorSet);
		LightningFlash.sky = new Color(0.9f, 0.95f, 1f);
		LightningFlash.shadow = new Color(40f / 51f, 0.8235294f, 72f / 85f);
		LightningFlash.weatherOverlays = new Color(0.9f, 0.95f, 1f);
	}
}
