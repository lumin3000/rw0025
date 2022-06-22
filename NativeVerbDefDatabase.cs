using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NativeVerbDefDatabase
{
	public static List<VerbDefinition> allVerbDefs;

	static NativeVerbDefDatabase()
	{
		allVerbDefs = VerbDefsHardcodedNative.AllVerbDefinitions().ToList();
	}

	public static VerbDefinition VerbWithID(VerbID id)
	{
		VerbDefinition verbDefinition = allVerbDefs.Where((VerbDefinition v) => v.id == id).FirstOrDefault();
		if (verbDefinition == null)
		{
			Debug.LogError("Failed to find Verb with id " + id);
		}
		return verbDefinition;
	}
}
