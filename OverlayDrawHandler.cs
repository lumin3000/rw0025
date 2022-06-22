using UnityEngine;

public static class OverlayDrawHandler
{
	private static int lastHomeZoneDrawFrame;

	private static int lastPowerGridDrawFrame;

	public static bool ShouldDrawHomeZone => lastHomeZoneDrawFrame + 1 >= Time.frameCount;

	public static bool ShouldDrawPowerGrid => lastPowerGridDrawFrame + 1 >= Time.frameCount;

	public static void DrawHomeZoneOverlay()
	{
		lastHomeZoneDrawFrame = Time.frameCount;
	}

	public static void DrawPowerGridOverlay()
	{
		lastPowerGridDrawFrame = Time.frameCount;
	}
}
