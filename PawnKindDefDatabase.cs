using System.Collections.Generic;

public static class PawnKindDefDatabase
{
	private static Dictionary<string, PawnKindDefinition> allKindDefs;

	public static IEnumerable<PawnKindDefinition> AllKindDefs
	{
		get
		{
			foreach (KeyValuePair<string, PawnKindDefinition> allKindDef in allKindDefs)
			{
				yield return allKindDef.Value;
			}
		}
	}

	static PawnKindDefDatabase()
	{
		allKindDefs = new Dictionary<string, PawnKindDefinition>();
		foreach (PawnKindDefinition item in PawnKindDefsHardcoded.AllPawnKindDefsHardcoded())
		{
			allKindDefs.Add(item.kindLabel, item);
		}
	}

	public static PawnKindDefinition KindDefNamed(string kindDefLabel)
	{
		return allKindDefs[kindDefLabel];
	}
}
