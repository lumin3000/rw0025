using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapInitParams
{
	public static bool startedFromEntry;

	public static List<Pawn> colonists;

	public static string mapToLoad;

	public static Storyteller chosenStoryteller;

	public static int mapSize;

	public static List<Storyteller> allStorytellers;

	private static int loadCount;

	public static bool StartedDebug => !StartedFromMapLoad && !startedFromEntry;

	public static bool StartedFromMapLoad => mapToLoad != string.Empty;

	static MapInitParams()
	{
		mapSize = 200;
		Reset();
	}

	public static void Reset()
	{
		loadCount = 0;
		startedFromEntry = false;
		mapToLoad = string.Empty;
		ThingIDCounter.Reset();
		colonists = new List<Pawn>();
		for (int i = 0; i < 3; i++)
		{
			colonists.Add(PawnMaker.GeneratePawn("Colonist", TeamType.Colonist));
		}
		allStorytellers = (from type in typeof(Storyteller).AllLeafSubclasses()
			select Activator.CreateInstance(type)).Cast<Storyteller>().ToList();
		chosenStoryteller = allStorytellers[0];
	}

	public static void Notify_MapInited()
	{
		loadCount++;
		if (loadCount > 1)
		{
			Debug.LogWarning("Failed to reset map init params.");
		}
	}
}
