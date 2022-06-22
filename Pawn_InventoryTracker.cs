using System;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_InventoryTracker : Saveable
{
	protected Pawn pawn;

	protected List<Thing> inventoryList = new List<Thing>();

	public Pawn_InventoryTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe.LookList(ref inventoryList, "InventoryList");
	}

	public void Take(Thing newInv, int countToTake)
	{
		Thing newInv2 = newInv.SplitOff(countToTake);
		Take(newInv2);
	}

	public void Take(Thing newInv)
	{
		if (newInv.spawnedInWorld)
		{
			newInv.DeSpawn();
		}
		if (newInv.def.stackLimit > 1)
		{
			foreach (Thing inventory in inventoryList)
			{
				if (inventory.StacksWith(newInv) && inventory.stackCount < inventory.def.stackLimit)
				{
					int num = Math.Min(newInv.stackCount, inventory.def.stackLimit - inventory.stackCount);
					inventory.stackCount += num;
					newInv.stackCount -= num;
				}
				if (newInv.stackCount <= 0)
				{
					break;
				}
			}
		}
		if (newInv.stackCount > 0)
		{
			inventoryList.Add(newInv);
		}
	}

	public void DestroyInventory(EntityType oldInvType)
	{
		if (inventoryList.RemoveAll((Thing t) => t.def.eType == oldInvType) == 0)
		{
			Debug.LogWarning(string.Concat(pawn, " tried to destroy all inventory of type ", oldInvType, " but they had none."));
		}
	}

	public void DestroyInventory(Thing oldInv)
	{
		inventoryList.Remove(oldInv);
	}

	public List<Thing> GetInventoryListForReading()
	{
		return inventoryList;
	}

	public void Drop(Thing t)
	{
		if (inventoryList.Contains(t))
		{
			inventoryList.Remove(t);
			ThingMaker.Spawn(t, pawn.Position, IntRot.random);
		}
	}

	public void Drop(Thing t, int countToDrop)
	{
		if (t.stackCount >= countToDrop)
		{
			if (countToDrop == t.stackCount)
			{
				Drop(t);
			}
			else
			{
				ThingMaker.Spawn(t.SplitOff(countToDrop), pawn.Position, IntRot.random);
			}
		}
	}

	public void Drop(EntityType TypeToDrop, int CountToDrop)
	{
		if (!Has(TypeToDrop, CountToDrop))
		{
			return;
		}
		foreach (Thing inventory in inventoryList)
		{
			if (inventory.def.eType == TypeToDrop)
			{
				int num = Math.Min(inventory.stackCount, CountToDrop);
				Drop(inventory, num);
				CountToDrop -= num;
			}
			if (CountToDrop <= 0)
			{
				break;
			}
		}
	}

	public bool Has(EntityType tType)
	{
		return Has(tType, 1);
	}

	public bool Has(EntityType tType, int countNeeded)
	{
		int num = 0;
		foreach (Thing inventory in inventoryList)
		{
			if (inventory.def.eType == tType)
			{
				num += inventory.stackCount;
			}
			if (num >= countNeeded)
			{
				return true;
			}
		}
		return false;
	}

	public int NumCarried(EntityType tType)
	{
		int num = 0;
		foreach (Thing inventory in inventoryList)
		{
			if (inventory.def.eType == tType)
			{
				num += inventory.stackCount;
			}
		}
		return num;
	}

	public void DropAll()
	{
		List<Thing> list = new List<Thing>();
		foreach (Thing inventory in inventoryList)
		{
			list.Add(inventory);
		}
		foreach (Thing item in list)
		{
			Drop(item);
		}
		inventoryList.Clear();
	}
}
