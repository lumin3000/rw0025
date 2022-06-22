using System.Collections.Generic;
using UnityEngine;

public class SlotGroup : Saveable
{
	public Building_Storage building;

	public Dictionary<StoreType, bool> acceptSettings = new Dictionary<StoreType, bool>();

	public bool acceptColonistCorpses;

	public bool acceptStrangerCorpses;

	public bool acceptAnimalCorpses = true;

	public IEnumerable<Thing> HeldThings
	{
		get
		{
			foreach (IntVec3 sq in Gen.SquaresOccupiedBy(building))
			{
				Thing st = sq.ContainedStorable();
				if (st != null)
				{
					yield return st;
				}
			}
		}
	}

	public IEnumerable<IntVec3> Squares
	{
		get
		{
			if (building.def.eType == EntityType.Building_Grave)
			{
				yield return building.Position;
				yield break;
			}
			int numReturned = 0;
			foreach (IntVec3 sq in Gen.SquaresOccupiedBy(building))
			{
				if (numReturned >= building.def.maxNumStoreSlots)
				{
					break;
				}
				yield return sq;
				numReturned++;
			}
		}
	}

	public SlotGroup(Building_Storage building)
	{
		this.building = building;
		if (building is Building_Grave)
		{
			acceptColonistCorpses = true;
			acceptStrangerCorpses = true;
			acceptAnimalCorpses = false;
		}
	}

	public IEnumerator<IntVec3> GetEnumerator()
	{
		foreach (IntVec3 square in Squares)
		{
			yield return square;
		}
	}

	public void Notify_BuildingSpawned()
	{
		foreach (StoreType storable in building.def.storables)
		{
			acceptSettings.Add(storable, value: true);
		}
		Find.SlotGroupManager.AddGroup(this);
	}

	public void Notify_BuildingDestroyed()
	{
		Find.SlotGroupManager.RemoveGroup(this);
	}

	public void ExposeData()
	{
		Scribe.LookField(ref acceptColonistCorpses, "AcceptColonistCorpses");
		Scribe.LookField(ref acceptStrangerCorpses, "AcceptStrangerCorpses");
		Scribe.LookField(ref acceptAnimalCorpses, "AcceptAnimalCorpses");
	}

	public bool AllowedToAccept(Thing storeThing)
	{
		Corpse corpse = storeThing as Corpse;
		if (corpse != null)
		{
			if (corpse.sourcePawn.raceDef.humanoid)
			{
				if (corpse.sourcePawn.Team == TeamType.Colonist)
				{
					if (!acceptColonistCorpses)
					{
						return false;
					}
				}
				else if (!acceptStrangerCorpses)
				{
					return false;
				}
			}
			else if (!acceptAnimalCorpses)
			{
				return false;
			}
		}
		if (!building.def.storables.Contains(storeThing.def.storeType))
		{
			return false;
		}
		return acceptSettings[storeThing.def.storeType];
	}

	public IEnumerable<Command> GetSlotConfigCommands()
	{
		yield return new Command_Toggle
		{
			hotKey = KeyCode.C,
			isActive = () => acceptColonistCorpses,
			icon = Res.LoadTexture("UI/Commands/AcceptCorpsesColonist"),
			tipDef = new TooltipDef("Allow placing colonist corpses here."),
			action = delegate
			{
				acceptColonistCorpses = !acceptColonistCorpses;
			}
		};
		yield return new Command_Toggle
		{
			hotKey = KeyCode.V,
			isActive = () => acceptStrangerCorpses,
			icon = Res.LoadTexture("UI/Commands/AcceptCorpsesStranger"),
			tipDef = new TooltipDef("Allow placing strangers' corpses here."),
			action = delegate
			{
				acceptStrangerCorpses = !acceptStrangerCorpses;
			}
		};
		yield return new Command_Toggle
		{
			hotKey = KeyCode.B,
			isActive = () => acceptAnimalCorpses,
			icon = Res.LoadTexture("UI/Commands/AcceptCorpsesAnimal"),
			tipDef = new TooltipDef("Allow placing animals' corpses here."),
			action = delegate
			{
				acceptAnimalCorpses = !acceptAnimalCorpses;
			}
		};
	}
}
