using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StorageUtility
{
	public static bool IsInStorage(this Thing t)
	{
		return t.StoringBuilding() != null;
	}

	public static Building StoringBuilding(this Thing t)
	{
		if (!t.spawnedInWorld)
		{
			return null;
		}
		return t.Position.ContainingSlotGroup()?.building;
	}

	public static bool StorageIsValid(this Thing t)
	{
		SlotGroup slotGroup = t.Position.ContainingSlotGroup();
		if (slotGroup == null)
		{
			Debug.LogError(string.Concat("Calling StorageIsValid on ", t, " which isn't in storage."));
			return false;
		}
		return slotGroup.AllowedToAccept(t);
	}

	public static SlotGroup ContainingSlotGroup(this IntVec3 loc)
	{
		return Find.SlotGroupManager.SlotGroupAt(loc);
	}

	public static Thing ContainedStorable(this IntVec3 loc)
	{
		foreach (Thing item in Find.Grids.ThingsAt(loc))
		{
			if (item.def.Storeable)
			{
				return item;
			}
		}
		return null;
	}

	public static bool IsValidStorageFor(this IntVec3 sq, Thing storable)
	{
		if (!CanStoreAnythingIn(sq))
		{
			return false;
		}
		SlotGroup slotGroup = sq.ContainingSlotGroup();
		if (slotGroup == null)
		{
			return false;
		}
		if (!slotGroup.AllowedToAccept(storable))
		{
			return false;
		}
		return true;
	}

	private static bool CanStoreAnythingIn(IntVec3 sq)
	{
		foreach (Thing item in Find.Grids.ThingsAt(sq))
		{
			if (item.def.Storeable)
			{
				return false;
			}
			if (item.def.passability != 0 && item.def.storables.Count == 0)
			{
				return false;
			}
		}
		return true;
	}

	public static IntVec3 ClosestAvailableStorageSquareFor(Thing storeThing, out bool succeeded)
	{
		ReservationManager resManager = Find.ReservationManager;
		IEnumerable<IntVec3> enumerable = from g in Find.SlotGroupManager.AllGroups
			where g.AllowedToAccept(storeThing)
			from sq in g.Squares.Cast<IntVec3>()
			where CanStoreAnythingIn(sq) && resManager.ReserverOf(sq, ReservationType.Store) == null && storeThing.CanReach(sq, adjacentIsOK: true)
			select sq;
		float num = 2.1474836E+09f;
		IntVec3 result = default(IntVec3);
		succeeded = false;
		foreach (IntVec3 item in enumerable)
		{
			float lengthHorizontalSquared = (storeThing.Position - item).LengthHorizontalSquared;
			if (lengthHorizontalSquared < num)
			{
				succeeded = true;
				result = item;
				num = lengthHorizontalSquared;
			}
		}
		return result;
	}
}
