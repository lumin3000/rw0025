using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class RaceDefinitionLoader
{
	public static IEnumerable<RaceDefinition> AllRaceDefinitions()
	{
		MethodInfo[] methods = typeof(RaceDefsHardcoded).GetMethods();
		foreach (MethodInfo method in methods)
		{
			if (!method.Name.StartsWith("Definitions_"))
			{
				continue;
			}
			foreach (RaceDefinition item in (IEnumerable)method.Invoke(null, null))
			{
				yield return item;
			}
		}
	}
}
