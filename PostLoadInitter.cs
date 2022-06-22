using System.Collections.Generic;
using UnityEngine;

public static class PostLoadInitter
{
	private static HashSet<Saveable> saveablesToPostLoad = new HashSet<Saveable>();

	public static void RegisterForPostLoadInit(Saveable s)
	{
		if (s == null || saveablesToPostLoad.Contains(s))
		{
			Debug.LogWarning(string.Concat("Bad register ", s, " in RegisterforPostLoadInit."));
		}
		else
		{
			saveablesToPostLoad.Add(s);
		}
	}

	public static void DoAllInits()
	{
		foreach (Saveable item in saveablesToPostLoad)
		{
			item.ExposeData();
		}
		saveablesToPostLoad.Clear();
	}
}
