using System.Collections.Generic;
using System.Linq;

public static class EquipmentFinderUtility
{
	public static IEnumerable<Equipment> AllStoredEquipment
	{
		get
		{
			foreach (Building_EquipmentRack rack in Find.BuildingManager.AllBuildingsColonistOfClass<Building_EquipmentRack>())
			{
				foreach (Equipment item in rack.slotGroup.HeldThings.Where((Thing t) => t is Equipment).Cast<Equipment>())
				{
					yield return item;
				}
			}
		}
	}
}
