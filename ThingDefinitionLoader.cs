using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ThingDefinitionLoader
{
	public static void LoadThingDefinitions()
	{
		List<ThingDefinition> list = HardcodedThingDefs().Concat(MoteDefLoader.MoteDefsFromTextures()).Concat(GunAndBulletDefs()).Concat(DataLoader.LoadDataInFolder<ThingDefinition>("Things"))
			.ToList();
		foreach (ThingDefinition item in list)
		{
			item.PostLoad();
			ThingDefDatabase.AddDefinitionToDatabase(item);
		}
		List<ThingDefinition> list2 = ConstructionUtility.ConstructionRelatedThingDefinitions().Concat(SeedDefinitionMaker.SeedDefinitions()).ToList();
		foreach (ThingDefinition item2 in list2)
		{
			item2.PostLoad();
			ThingDefDatabase.AddDefinitionToDatabase(item2);
		}
		foreach (ThingDefinition allThingDefinition in ThingDefDatabase.AllThingDefinitions)
		{
			allThingDefinition.ErrorCheck();
		}
	}

	private static IEnumerable<ThingDefinition> HardcodedThingDefs()
	{
		MethodInfo[] methods = typeof(ThingDefsHardcoded).GetMethods();
		foreach (MethodInfo method in methods)
		{
			if (!method.Name.StartsWith("Definitions_"))
			{
				continue;
			}
			foreach (ThingDefinition item in (IEnumerable)method.Invoke(null, null))
			{
				yield return item;
			}
		}
	}

	private static IEnumerable<ThingDefinition> GunAndBulletDefs()
	{
		yield break;
	}
}
