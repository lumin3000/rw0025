using UnityEngine;

public static class MapIniter
{
	public static void FinalizeMapInit(bool loaded)
	{
		if (!loaded)
		{
			Find.FogGrid.RemakeEntireFogGrid();
		}
		Find.Map.initialized = true;
		Find.PathGrid.RecalculateAllPathCosts();
		PowerNetManager.UpdatePowerNetsAndConnections_First();
		Find.Map.mapDrawer.RegenerateEverythingNow();
		Find.ResearchManager.ReapplyAllMods();
		if (!loaded)
		{
			Find.CameraMap.JumpTo(Genner_PlayerStuff.PlayerStartSpot);
		}
		Find.CameraFade.StartFade(Color.black, 0f);
		Find.CameraFade.StartFade(Color.clear, 0.5f);
	}
}
