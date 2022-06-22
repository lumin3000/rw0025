using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager
{
	public HashSet<Building> AllBuildingsArtificial = new HashSet<Building>();

	public List<Building> AllBuildingsColonist = new List<Building>();

	public HashSet<Building> AllBuildingsColonistCombatTargets = new HashSet<Building>();

	public HashSet<Building> AllBuildingsColonistElecFire = new HashSet<Building>();

	public HashSet<Building> AllBuildingsOwned = new HashSet<Building>();

	public HashSet<Weapon_Antimissile> AllAntimissiles = new HashSet<Weapon_Antimissile>();

	public HashSet<Weapon> AllWeapons = new HashSet<Weapon>();

	public void RegisterBuilding(Building b)
	{
		if (b.def.naturalBuilding)
		{
			return;
		}
		AllBuildingsArtificial.Add(b);
		if (b.Team == TeamType.Colonist)
		{
			AllBuildingsColonist.Add(b);
			if (b.def.combatTargetBuilding)
			{
				AllBuildingsColonistCombatTargets.Add(b);
			}
		}
		if (b.Team != 0)
		{
			AllBuildingsOwned.Add(b);
		}
		if (b.def.startElectricalFires)
		{
			AllBuildingsColonistElecFire.Add(b);
		}
		Weapon weapon = b as Weapon;
		if (weapon != null)
		{
			AllWeapons.Add(weapon);
		}
		if (b is Weapon_Antimissile)
		{
			AllAntimissiles.Add(b as Weapon_Antimissile);
		}
	}

	public void DeRegisterBuilding(Building b)
	{
		AllBuildingsArtificial.Remove(b);
		if (b.Team == TeamType.Colonist)
		{
			AllBuildingsColonist.Remove(b);
			if (b.def.combatTargetBuilding)
			{
				AllBuildingsColonistCombatTargets.Remove(b);
			}
		}
		if (b.Team != 0)
		{
			AllBuildingsOwned.Remove(b);
		}
		if (b.def.startElectricalFires)
		{
			AllBuildingsColonistElecFire.Remove(b);
		}
		Weapon weapon = b as Weapon;
		if (weapon != null)
		{
			AllWeapons.Remove(weapon);
		}
		if (b is Weapon_Antimissile)
		{
			AllAntimissiles.Remove(b as Weapon_Antimissile);
		}
	}

	public Building RandomBuildingColonistOfType(EntityType BType)
	{
		if (!PlayerHasBuildingOfType(BType))
		{
			return null;
		}
		return AllBuildingsColonistOfType(BType).ToList().RandomElement();
	}

	public bool PlayerHasBuildingOfType(EntityType BType)
	{
		return AllBuildingsColonistOfType(BType).Any();
	}

	public bool PlayerHasBuildingOfDef(ThingDefinition def)
	{
		return AllBuildingsColonist.Where((Building b) => b.def == def).Any();
	}

	public IEnumerable<Building> AllBuildingsColonistOfType(EntityType BType)
	{
		foreach (Building b in AllBuildingsColonist)
		{
			if (b.def.eType == BType)
			{
				yield return b;
			}
		}
	}

	public IEnumerable<Building> AllBuildingsColonistOfDef(ThingDefinition def)
	{
		foreach (Building b in AllBuildingsColonist)
		{
			if (b.def == def)
			{
				yield return b;
			}
		}
	}

	public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
	{
		foreach (Building b in AllBuildingsColonist)
		{
			T casted = b as T;
			if (casted != null)
			{
				yield return casted;
			}
		}
	}

	public Building BuildingAtSquare(IntVec3 Square)
	{
		foreach (Thing item in Find.Grids.ThingsAt(Square))
		{
			Building building = item as Building;
			if (building != null)
			{
				return building;
			}
		}
		return null;
	}

	public Building RandomBuildingPlayer()
	{
		return AllBuildingsColonist.RandomElement();
	}

	public IntVec3 RandomImpactPosition()
	{
		Building building;
		do
		{
			building = RandomBuildingPlayer();
		}
		while (!building.def.useStandardHealth);
		return building.Position;
	}

	public IntVec3 TradeDropLocation()
	{
		IntVec3 loc = IntVec3.Invalid;
		foreach (Building item in Find.BuildingManager.AllBuildingsColonist)
		{
			if (item.def.eType == EntityType.Building_DropBeacon && !Find.RoofGrid.Roofed(item.Position))
			{
				loc = item.Position;
				break;
			}
		}
		if (loc.IsInvalid)
		{
			loc = GeneratedTradeDropSpot();
		}
		return DropPodUtility.DropPodSpotNear(loc, 3);
	}

	private IntVec3 GeneratedTradeDropSpot()
	{
		Predicate<IntVec3> validator = (IntVec3 sq) => sq.Standable() && !Find.Grids.SquareContains(sq, EntityCategory.SmallObject) && !Find.Grids.SquareContains(sq, EntityCategory.Building) && !Find.RoofGrid.Roofed(sq);
		IntVec3 position = RandomBuildingColonistOfType(EntityType.Building_CommsConsole).Position;
		int num = 10;
		IntVec3 result;
		bool succeeded;
		do
		{
			result = GenMap.RandomMapSquareNear(position, num, validator, out succeeded);
			num = Mathf.RoundToInt((float)num * 1.5f);
			if (num > Find.Map.Size.x)
			{
				Debug.LogWarning("Failed to find proper trade drop spot");
				return GenMap.RandomSquareWith(validator);
			}
		}
		while (!succeeded);
		return result;
	}
}
