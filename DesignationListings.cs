using System.Collections.Generic;
using System.Linq;

public static class DesignationListings
{
	private static IEnumerable<EntityDefinition> AllDefsWithDesCategory(DesignationCategory cat)
	{
		return ThingDefDatabase.AllThingDefinitions.Where((ThingDefinition def) => def.designationCategory == cat).Cast<EntityDefinition>().Concat(TerrainDefDatabase.allTerrainDefs.Where((TerrainDefinition def) => def.designationCategory == cat).Cast<EntityDefinition>());
	}

	public static IEnumerable<Designator> ListOrders()
	{
		yield return new Designator_Cancel();
		yield return new Designator_Sell();
		yield return new Designator_EmptySpace();
		yield return new Designator_Mine();
		yield return new Designator_Haul();
		yield return new Designator_CutPlants();
		yield return new Designator_HarvestPlants();
		yield return new Designator_Clean(shouldClean: true);
		yield return new Designator_Clean(shouldClean: false);
	}

	public static IEnumerable<Designator> ListStructure()
	{
		yield return new Designator_Cancel();
		yield return new Designator_Sell();
		yield return new Designator_EmptySpace();
		foreach (EntityDefinition ent in AllDefsWithDesCategory(DesignationCategory.Structure))
		{
			yield return new Designator_Place(ent);
		}
	}

	public static IEnumerable<Designator> ListAreas()
	{
		yield return new Designator_Cancel();
		yield return new Designator_Sell();
		yield return new Designator_EmptySpace();
		foreach (EntityDefinition ent in AllDefsWithDesCategory(DesignationCategory.Area))
		{
			yield return new Designator_Place(ent);
		}
	}

	public static IEnumerable<Designator> ListFurniture()
	{
		yield return new Designator_Cancel();
		yield return new Designator_Sell();
		yield return new Designator_EmptySpace();
		foreach (EntityDefinition ent in AllDefsWithDesCategory(DesignationCategory.Furniture))
		{
			yield return new Designator_Place(ent);
		}
	}

	public static IEnumerable<Designator> ListBuildings()
	{
		yield return new Designator_Cancel();
		yield return new Designator_Sell();
		yield return new Designator_EmptySpace();
		foreach (EntityDefinition ent in AllDefsWithDesCategory(DesignationCategory.Building))
		{
			yield return new Designator_Place(ent);
		}
	}

	public static IEnumerable<Designator> ListSecurity()
	{
		yield return new Designator_Cancel();
		yield return new Designator_Sell();
		yield return new Designator_EmptySpace();
		foreach (EntityDefinition ent in AllDefsWithDesCategory(DesignationCategory.Security))
		{
			yield return new Designator_Place(ent);
		}
	}
}
