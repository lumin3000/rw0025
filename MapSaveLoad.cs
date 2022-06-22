using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class MapSaveLoad
{
	public static void SaveToFile(Map MapToSave, string mapName)
	{
		using (Stream stream = new FileStream(MapFiles.FilePathForMap(mapName), FileMode.Create, FileAccess.Write, FileShare.None))
		{
			Scribe.mode = LoadSaveMode.Saving;
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.IndentChars = "\t";
			using (Scribe.writer = XmlWriter.Create(stream, xmlWriterSettings))
			{
				Scribe.writer.WriteStartDocument();
				Scribe.EnterNode("Map");
				string value = VersionControl.versionStringFull;
				Scribe.LookField(ref value, "GameVersion");
				Scribe.LookSaveable(ref Find.Map.info, "MapInfo");
				MapFileCompressor mapFileCompressor = new MapFileCompressor();
				mapFileCompressor.PrepareDataForSave();
				ExposeMapComponents();
				mapFileCompressor.ExposeData();
				Scribe.EnterNode("Things");
				foreach (Thing spawnedThing in MapToSave.thingLister.spawnedThings)
				{
					if (spawnedThing.def.isSaveable && !spawnedThing.IsSaveCompressible())
					{
						Thing target = spawnedThing;
						Scribe.LookSaveable(ref target, "Thing");
					}
				}
				Scribe.ExitNode();
				Scribe.ExitNode();
				Scribe.writer.WriteEndDocument();
			}
		}
		Scribe.mode = LoadSaveMode.None;
	}

	public static void FillMapFromFile(string mapName)
	{
		StreamReader input = new StreamReader(MapFiles.FilePathForMap(mapName));
		XmlTextReader xmlReader = new XmlTextReader(input);
		Scribe.doc = new XmlDocument();
		Scribe.doc.Load(xmlReader);
		Scribe.mode = LoadSaveMode.LoadingVars;
		Scribe.curNode = Scribe.doc.SelectSingleNode("/Map");
		string value = string.Empty;
		Scribe.LookField(ref value, "GameVersion");
		if (value != VersionControl.versionStringFull)
		{
			Debug.LogWarning("Version mismatch: Map file is version " + value + ", we are running version " + VersionControl.versionStringFull + ".");
		}
		Scribe.LookSaveable(ref Find.Map.info, "MapInfo");
		Find.Map.InitComponents();
		ExposeMapComponents();
		if (!MapFiles.IsAutoSave(mapName))
		{
			Find.Map.info.fileName = mapName;
		}
		MapFileCompressor mapFileCompressor = new MapFileCompressor();
		mapFileCompressor.ExposeData();
		List<Thing> second = mapFileCompressor.ThingsToSpawnAfterLoad().ToList();
		List<Thing> valueList = new List<Thing>();
		Scribe.LookList(ref valueList, "Things");
		foreach (Thing item in valueList.Concat(second))
		{
			ThingMaker.Spawn(item, item.Position, item.rotation);
		}
		Scribe.ExitNode();
		Scribe.mode = LoadSaveMode.ResolvingCrossRefs;
		ThingRefHandler.ResolveCrossRefs();
		Scribe.mode = LoadSaveMode.PostLoadInit;
		PostLoadInitter.DoAllInits();
		Scribe.mode = LoadSaveMode.None;
	}

	private static void ExposeMapComponents()
	{
		Scribe.LookSaveable(ref Find.Map.colonyInfo, "ColonyInfo");
		Scribe.LookSaveable(ref Find.Map.playSettings, "PlaySettings");
		Scribe.LookSaveable(ref Find.GameRoot.realTime, "RealTime");
		Scribe.LookSaveable(ref Find.Map.gameEnder, "GameEnder");
		Scribe.LookSaveable(ref Find.Map.letterStack, "LetterStack");
		Find.CameraMap.Expose();
		Find.ResourceManager.Expose();
		Find.TickManager.Expose();
		Scribe.LookSaveable(ref Find.Map.weatherManager, "WeatherManager");
		Scribe.LookSaveable(ref Find.Map.researchManager, "ResearchManager");
		Scribe.LookSaveable(ref Find.Map.storyteller, "Storyteller");
		Scribe.LookSaveable(ref Find.Map.reservationManager, "ReservationManager");
		Scribe.LookSaveable(ref Find.Map.designationManager, "DesignationManager");
		Scribe.LookSaveable(ref Find.Map.aiKingManager, "AIKingManager");
		Scribe.LookSaveable(ref Find.Map.visitorManager, "VisitorManager");
		Scribe.LookSaveable(ref Find.Map.tutor, "Tutor");
		Scribe.LookSaveable(ref Find.Map.mapConditionManager, "MapConditionManager");
		Find.Map.roomManager.Expose();
		Scribe.LookSaveable(ref Find.Map.fogGrid, "FogGrid");
		Scribe.LookSaveable(ref Find.Map.roofGrid, "RoofGrid");
		Scribe.LookSaveable(ref Find.Map.terrainGrid, "TerrainGrid");
		Scribe.LookSaveable(ref Find.Map.homeZoneGrid, "CleanGrid");
		Scribe.LookSaveable(ref Find.Map.historicalPawns, "HistoricalPawns");
	}
}
