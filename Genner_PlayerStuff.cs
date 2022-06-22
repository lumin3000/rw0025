public static class Genner_PlayerStuff
{
	public static IntVec3 PlayerStartSpot => Find.Map.Center;

	public static void GenerateAllPlayerStartingStuff()
	{
		MakeStartingBuildings();
		SpawnStartingColonists();
		GetStartingResources();
		GetStartingWeapons();
	}

	private static void MakeStartingBuildings()
	{
		IntVec3 playerStartSpot = PlayerStartSpot;
		Thing thing = ThingMaker.MakeThing(EntityType.Area_Stockpile);
		thing.Team = TeamType.Colonist;
		ThingMaker.Spawn(thing, playerStartSpot + new IntVec3(8, 0, 0));
		Thing thing2 = ThingMaker.MakeThing("SleepingSpot");
		thing2.Team = TeamType.Colonist;
		ThingMaker.Spawn(thing2, playerStartSpot + new IntVec3(2, 0, -6));
		thing2 = ThingMaker.MakeThing("SleepingSpot");
		thing2.Team = TeamType.Colonist;
		ThingMaker.Spawn(thing2, playerStartSpot + new IntVec3(5, 0, -5), IntRot.south);
		thing2 = ThingMaker.MakeThing("SleepingSpot");
		thing2.Team = TeamType.Colonist;
		ThingMaker.Spawn(thing2, playerStartSpot + new IntVec3(3, 0, -8), IntRot.east);
		Thing thing3 = ThingMaker.MakeThing(EntityType.Area_Dump);
		thing3.Team = TeamType.Colonist;
		ThingMaker.Spawn(thing3, playerStartSpot + new IntVec3(-5, 0, -3));
	}

	public static void SpawnStartingColonists()
	{
		foreach (Pawn colonist in MapInitParams.colonists)
		{
			IntVec3 newThingPos = GenMap.RandomStandableLOSSquareNear(PlayerStartSpot, 3);
			if (MapInitParams.StartedDebug)
			{
				ThingMaker.Spawn(colonist, newThingPos);
			}
			else
			{
				DropPodContentsInfo contents = new DropPodContentsInfo(colonist);
				DropPodIncoming dropPodIncoming = (DropPodIncoming)ThingMaker.MakeThing(EntityType.DropPodIncoming);
				dropPodIncoming.contents = contents;
				ThingMaker.Spawn(dropPodIncoming, newThingPos);
			}
			colonist.psychology.thoughts.GainThought(ThoughtType.NewColonyOptimism);
		}
	}

	public static void GetStartingResources()
	{
		Find.ResourceManager.Money += 1000;
		Find.ResourceManager.Food += 50;
		Find.ResourceManager.Metal += 500;
	}

	public static void GetStartingWeapons()
	{
		Equipment newThing = (Equipment)ThingMaker.MakeThing("Gun_Pistol");
		IntVec3 newThingPos = Find.Map.Center + new IntVec3(0, 0, -5);
		ThingMaker.Spawn(newThing, newThingPos);
	}
}
