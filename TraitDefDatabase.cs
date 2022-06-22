using System.Collections.Generic;

public static class TraitDefDatabase
{
	public static List<TraitDefinition> allTraitDefs;

	static TraitDefDatabase()
	{
		allTraitDefs = new List<TraitDefinition>();
		foreach (TraitDefinition item in TraitDefinitionsHardcoded.AllHardcodedTraitDefinitions())
		{
			allTraitDefs.Add(item);
		}
	}

	public static TraitDefinition DefinitionOf(TraitEffect tType)
	{
		foreach (TraitDefinition allTraitDef in allTraitDefs)
		{
			if (allTraitDef.effect == tType)
			{
				return allTraitDef;
			}
		}
		return null;
	}

	public static TraitDefinition DefinitionWithLabel(string lab)
	{
		foreach (TraitDefinition allTraitDef in allTraitDefs)
		{
			if (allTraitDef.label == lab)
			{
				return allTraitDef;
			}
		}
		return null;
	}
}
