using System.Collections.Generic;
using UnityEngine;

public static class LoadedThingLookup
{
	private static List<Thing> allThingsConstructedThisLoad = new List<Thing>();

	private static Dictionary<int, Thing> allThingsByID = new Dictionary<int, Thing>();

	public static void RegisterThingConstructed(Thing t)
	{
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			allThingsConstructedThisLoad.Add(t);
		}
	}

	public static void InitThingIDLookup()
	{
		allThingsByID.Clear();
		foreach (Thing item in allThingsConstructedThisLoad)
		{
			if (allThingsByID.ContainsKey(item.thingIDNumber))
			{
				Debug.LogError(string.Concat("Thing ", item, " has a duplicate ID: ", item.thingIDNumber));
			}
			else if (item.def.HasThingIDNumber)
			{
				allThingsByID.Add(item.thingIDNumber, item);
			}
		}
		allThingsConstructedThisLoad.Clear();
	}

	public static Thing LoadedThingWithID(string id)
	{
		int num = Thing.IDNumberFromThingID(id);
		allThingsByID.TryGetValue(num, out var value);
		if (value != null)
		{
			return value;
		}
		Debug.LogWarning("Could not resolve reference to thing with ID " + id + " (num=" + num + "). Was it compressed away, destroyed, had no ID number, or not saved/loaded right?");
		return null;
	}
}
