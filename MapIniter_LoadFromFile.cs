public class MapIniter_LoadFromFile
{
	public static void InitMapFromFile(string MapName)
	{
		Find.GameRoot.GameReset();
		MapSaveLoad.FillMapFromFile(MapName);
		MapIniter.FinalizeMapInit(loaded: true);
	}
}
