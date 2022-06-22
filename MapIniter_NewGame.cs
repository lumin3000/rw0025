public static class MapIniter_NewGame
{
	public static void InitNewGeneratedMap()
	{
		Find.GameRoot.GameReset();
		Find.Map.info.size = new IntVec3(MapInitParams.mapSize, 1, MapInitParams.mapSize);
		Find.Map.InitComponents();
		Find.Map.storyteller = MapInitParams.chosenStoryteller;
		Find.Map.info.fileName = MapFiles.UnusedDefaultName();
		MapGenerator.GenerateNewMapContents();
		MapIniter.FinalizeMapInit(loaded: false);
		if (MapInitParams.startedFromEntry)
		{
			string text = "The three of you awake in your longsleep sarcophagi to the sound of sirens and ripping metal. You barely get to the escape pods before the ship is torn apart.\n\nA few hours later, you land on this unknown rimworld.\n\nAs pieces of the shredded starship fall around you, you start making plans to survive.";
			DiaNode diaNode = new DiaNode(text);
			DiaOption diaOption = new DiaOption();
			diaOption.PassTurnIfWorldMode = false;
			diaOption.ResolveTree = true;
			diaNode.optionList.Add(diaOption);
			DialogBoxHelper.InitDialogTree(diaNode);
		}
	}
}
