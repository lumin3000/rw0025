using System.Collections.Generic;

public static class RaceDefDatabase
{
	public static Dictionary<string, RaceDefinition> allRaces;

	static RaceDefDatabase()
	{
		allRaces = new Dictionary<string, RaceDefinition>();
		foreach (RaceDefinition item in RaceDefinitionLoader.AllRaceDefinitions())
		{
			allRaces.Add(item.raceName, item);
		}
	}

	public static RaceDefinition DefinitionNamed(string raceName)
	{
		return allRaces[raceName];
	}
}
