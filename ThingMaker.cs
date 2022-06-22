using System;
using UnityEngine;

public static class ThingMaker
{
	public static Thing MakeThing(EntityType eType)
	{
		return MakeThing(eType.DefinitionOfType());
	}

	public static Thing MakeThing(string defName)
	{
		return MakeThing(ThingDefDatabase.ThingDefNamed(defName));
	}

	public static T MakeThing<T>() where T : Thing
	{
		ThingDefinition thingDefinition = null;
		foreach (ThingDefinition allThingDefinition in ThingDefDatabase.AllThingDefinitions)
		{
			if (allThingDefinition.thingClass == typeof(T))
			{
				if (thingDefinition != null)
				{
					Debug.Log(string.Concat("Using MakeThing with class ", typeof(T), " when there is more than one ThingType with that class."));
				}
				thingDefinition = allThingDefinition;
			}
		}
		return (T)MakeThing(thingDefinition);
	}

	public static Thing MakeThing(ThingDefinition def)
	{
		Thing thing = Activator.CreateInstance(def.thingClass) as Thing;
		thing.def = def;
		ThingIDCounter.GiveIDTo(thing);
		if (thing.def.useStandardHealth)
		{
			thing.health = thing.def.maxHealth;
		}
		(thing as ThingWithComponents)?.SetupComponents();
		thing.ResolveRandomizedIcon();
		return thing;
	}

	public static Thing Spawn(string ThingDefName, IntVec3 newThingPos)
	{
		return Spawn(MakeThing(ThingDefDatabase.ThingDefNamed(ThingDefName)), newThingPos);
	}

	public static Thing Spawn(EntityType TType, IntVec3 newThingPos, IntRot newThingRot)
	{
		return Spawn(MakeThing(TType), newThingPos, newThingRot);
	}

	public static Thing Spawn(EntityType TType, IntVec3 newThingPos)
	{
		return Spawn(MakeThing(TType), newThingPos);
	}

	public static Thing Spawn(ThingDefinition ThingDef, IntVec3 newThingPos)
	{
		return Spawn(MakeThing(ThingDef), newThingPos);
	}

	public static Thing Spawn(Thing newThing, IntVec3 newThingPos)
	{
		return Spawn(newThing, newThingPos, IntRot.north);
	}

	public static Thing Spawn(Thing newThing)
	{
		return Spawn(newThing, newThing.Position);
	}

	public static Thing Spawn(Thing newThing, IntVec3 newThingPos, IntRot newThingRot)
	{
		if (newThing.def.stackLimit > 1)
		{
			Thing result = null;
			foreach (Thing item in Find.Grids.ThingsAt(newThingPos))
			{
				if (item.StacksWith(newThing) && item.stackCount < item.def.stackLimit)
				{
					int num = Math.Min(newThing.stackCount, item.def.stackLimit - item.stackCount);
					item.stackCount += num;
					newThing.stackCount -= num;
					result = item;
				}
				if (newThing.stackCount <= 0)
				{
					break;
				}
			}
			if (newThing.stackCount <= 0)
			{
				newThing.Destroy();
				return result;
			}
		}
		OverwriteExistingThings(newThingPos, newThingRot, newThing.def, reclaimResources: false);
		if (newThing.def.randomizeRotationOnSpawn)
		{
			newThing.rotation = IntRot.random;
		}
		else
		{
			newThing.rotation = newThingRot;
		}
		newThing.Position = newThingPos;
		newThing.SpawnSetup();
		return newThing;
	}

	public static void OverwriteExistingThings(IntVec3 newThingPos, IntRot newThingRot, EntityDefinition newDef, bool reclaimResources)
	{
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(newThingPos, newThingRot, newDef.size))
		{
			foreach (Thing item2 in Find.Grids.ThingsAt(item).ListFullCopy())
			{
				if (SpawningShouldWipe(newDef, item2.def))
				{
					if (reclaimResources)
					{
						GenMap.ReclaimResourcesFor(item2);
					}
					item2.Destroy();
				}
			}
		}
	}

	private static bool SpawningShouldWipe(EntityDefinition newEntDef, EntityDefinition oldEntDef)
	{
		ThingDefinition thingDefinition = newEntDef as ThingDefinition;
		ThingDefinition thingDefinition2 = oldEntDef as ThingDefinition;
		if (thingDefinition == null || thingDefinition2 == null)
		{
			return false;
		}
		if (!thingDefinition2.destroyable)
		{
			return false;
		}
		if (thingDefinition.IsPlant)
		{
			return false;
		}
		if (thingDefinition.eType == EntityType.DropPod)
		{
			return false;
		}
		if (thingDefinition2.eType == EntityType.Blueprint)
		{
			if (thingDefinition.eType == EntityType.Blueprint)
			{
				if (thingDefinition.entityDefToBuild.eType == EntityType.Door && thingDefinition2.entityDefToBuild.eType == EntityType.Wall)
				{
					return true;
				}
				if (thingDefinition2.entityDefToBuild.category == EntityCategory.Terrain)
				{
					if (thingDefinition.entityDefToBuild is ThingDefinition && thingDefinition.ThingDefToBuild.fillsSquare)
					{
						return true;
					}
					if (thingDefinition.entityDefToBuild.category == EntityCategory.Terrain)
					{
						return true;
					}
				}
			}
			if (thingDefinition2.entityDefToBuild.eType == EntityType.Building_PowerConduit && thingDefinition.entityDefToBuild is ThingDefinition && (thingDefinition.entityDefToBuild as ThingDefinition).transmitsPower)
			{
				return true;
			}
			return false;
		}
		if (thingDefinition.passability == Traversability.Impassable && thingDefinition2.category != EntityCategory.Pawn)
		{
			return true;
		}
		if (thingDefinition.IsBlocker)
		{
			if (thingDefinition2.isDebris)
			{
				return true;
			}
			if (thingDefinition2.IsPlant)
			{
				return true;
			}
		}
		if (thingDefinition.category == EntityCategory.Building && thingDefinition2.category == EntityCategory.Building)
		{
			return true;
		}
		return false;
	}
}
