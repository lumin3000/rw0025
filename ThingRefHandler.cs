using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ThingRefHandler
{
	private class CrossRefRequestSet
	{
		public Saveable requester;

		public List<string> wantedRefs = new List<string>();

		public List<List<string>> wantedRefLists = new List<List<string>>();

		public CrossRefRequestSet(Saveable requester)
		{
			this.requester = requester;
		}
	}

	private static List<CrossRefRequestSet> refRequests = new List<CrossRefRequestSet>();

	public static void RegisterDesiredCrossRef(Saveable requester, string refString)
	{
		CrossRefRequestSet requestSetFor = GetRequestSetFor(requester);
		requestSetFor.wantedRefs.Add(refString);
	}

	public static void RegisterDesiredCrossRefList(Saveable requester, List<string> refStringList)
	{
		CrossRefRequestSet requestSetFor = GetRequestSetFor(requester);
		requestSetFor.wantedRefLists.Add(refStringList);
	}

	public static void ResolveCrossRefs()
	{
		LoadedThingLookup.InitThingIDLookup();
		foreach (CrossRefRequestSet refRequest in refRequests)
		{
			refRequest.requester.ExposeData();
		}
		refRequests.Clear();
	}

	public static Thing NextResolvedRefFor(Saveable saveable)
	{
		CrossRefRequestSet requestSetFor = GetRequestSetFor(saveable);
		if (requestSetFor.wantedRefs.Count == 0)
		{
			Debug.LogWarning("Out of wanted refs for requester " + requestSetFor.requester);
			return null;
		}
		string thingName = requestSetFor.wantedRefs[0];
		requestSetFor.wantedRefs.RemoveAt(0);
		return ThingNamed(thingName);
	}

	public static List<Thing> NextResolvedRefListFor(Saveable saveable)
	{
		CrossRefRequestSet requestSetFor = GetRequestSetFor(saveable);
		if (requestSetFor.wantedRefLists.Count == 0)
		{
			Debug.LogWarning("Out of wanted ref lists for requester " + requestSetFor.requester);
			return null;
		}
		List<string> source = requestSetFor.wantedRefLists[0];
		requestSetFor.wantedRefLists.RemoveAt(0);
		return source.Select((string n) => ThingNamed(n)).ToList();
	}

	private static Thing ThingNamed(string thingName)
	{
		if (thingName == "null")
		{
			return null;
		}
		return LoadedThingLookup.LoadedThingWithID(thingName);
	}

	private static CrossRefRequestSet GetRequestSetFor(Saveable requester)
	{
		if (requester == null)
		{
			Debug.LogWarning("Null thing wanted cross ref.");
			return null;
		}
		foreach (CrossRefRequestSet refRequest in refRequests)
		{
			if (refRequest.requester == requester)
			{
				return refRequest;
			}
		}
		CrossRefRequestSet crossRefRequestSet = new CrossRefRequestSet(requester);
		refRequests.Add(crossRefRequestSet);
		return crossRefRequestSet;
	}
}
