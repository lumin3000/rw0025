public class UI_ThingOverlays
{
	public void ThingOverlaysOnGUI()
	{
		foreach (Thing spawnedGUIOverlayThing in Find.Map.thingLister.spawnedGUIOverlayThings)
		{
			if (!Find.FogGrid.IsFogged(spawnedGUIOverlayThing.Position))
			{
				spawnedGUIOverlayThing.DrawGUIOverlay();
			}
		}
	}
}
