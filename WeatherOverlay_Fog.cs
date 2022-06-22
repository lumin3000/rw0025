using UnityEngine;

public class WeatherOverlay_Fog : WeatherOverlay
{
	private static readonly Material FogOverlayScreen = MatLoader.LoadMat("Weather/FogOverlayScreen");

	private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/FogOverlayWorld");

	public WeatherOverlay_Fog()
	{
		worldOverlayMat = FogOverlayWorld;
		screenOverlayMat = FogOverlayScreen;
		worldOverlayPanSpeed1 = 0.0005f;
		worldOverlayPanSpeed2 = 0.0004f;
		worldPanDir1 = new Vector2(1f, 1f);
		worldPanDir2 = new Vector2(1f, -1f);
	}
}
