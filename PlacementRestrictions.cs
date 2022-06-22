using System.Collections.Generic;
using System.Linq;

public static class PlacementRestrictions
{
	public static AcceptanceReport CanPlaceWithRestriction(EntityDefinition checkingDef, PlacementRestriction rest, IntVec3 loc, IntRot rot)
	{
		if (rest == PlacementRestriction.NotAdjacentToDoor)
		{
			IEnumerable<Building> enumerable = Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Door).Concat(from bl in Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Blueprint)
				where bl.def.entityDefToBuild.eType == EntityType.Door
				select bl).Concat(from fr in Find.BuildingManager.AllBuildingsColonistOfType(EntityType.BuildingFrame)
				where fr.def.entityDefToBuild.eType == EntityType.Door
				select fr);
			foreach (Building item in enumerable)
			{
				if (item.Position.AdjacentTo8Way(loc))
				{
					return new AcceptanceReport("Doors cannot be placed in neighboring squares.");
				}
			}
		}
		if (rest == PlacementRestriction.OnSteamGeyser && Find.Grids.ThingAt(loc, EntityType.SteamGeyser) == null)
		{
			return new AcceptanceReport("Must be placed directly on a steam geyser.");
		}
		return true;
	}
}
