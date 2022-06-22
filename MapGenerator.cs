public static class MapGenerator
{
	public static Genner_Sanctity sanctity;

	public static void GenerateNewMapContents()
	{
		sanctity = new Genner_Sanctity();
		sanctity.GenerateGenZones();
		Genner_BaseTerrain genner_BaseTerrain = new Genner_BaseTerrain();
		genner_BaseTerrain.AddTallRocks();
		genner_BaseTerrain.AddRockDebris();
		genner_BaseTerrain.SetTerrains();
		Genner_Minerals genner_Minerals = new Genner_Minerals();
		genner_Minerals.AddThings();
		Genner_Geysers genner_Geysers = new Genner_Geysers();
		genner_Geysers.AddThings();
		IntVec3 loc = Find.Map.Center + new IntVec3(-5, 0, -11);
		genner_Minerals.AddThingAt(loc);
		Genner_DroppedResources genner_DroppedResources = new Genner_DroppedResources();
		genner_DroppedResources.AddThings();
		genner_DroppedResources.AddResourcePileAt(Find.Map.Center + new IntVec3(5, 0, 11), EntityType.Food, 4);
		genner_DroppedResources.AddResourcePileAt(Find.Map.Center + new IntVec3(-2, 0, 5), EntityType.Metal, 5);
		Genner_PlayerStuff.GenerateAllPlayerStartingStuff();
		Genner_Plants genner_Plants = new Genner_Plants();
		genner_Plants.AddPlants();
		Genner_Animals genner_Animals = new Genner_Animals();
		genner_Animals.AddAnimals();
	}
}
