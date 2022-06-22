using System.Collections.Generic;

public static class ScatterableDatabase
{
	public static List<ScatterableDefinition> allScatterableDefs;

	static ScatterableDatabase()
	{
		allScatterableDefs = DataLoader.LoadDataInFolder<ScatterableDefinition>("Terrain/Scatter");
	}

	public static ScatterableDefinition RandomScatterable()
	{
		return allScatterableDefs.RandomElementByWeight((ScatterableDefinition scat) => scat.selectionWeight);
	}
}
