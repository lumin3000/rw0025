using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotGroupManager
{
	private SlotGroup[,,] groupGrid;

	private List<SlotGroup> allGroups = new List<SlotGroup>();

	public IEnumerable<SlotGroup> AllGroups
	{
		get
		{
			foreach (SlotGroup allGroup in allGroups)
			{
				yield return allGroup;
			}
		}
	}

	public IEnumerable<IntVec3> AllSlots => from gr in allGroups
		from sq in gr.Squares
		select sq;

	public SlotGroupManager()
	{
		groupGrid = new SlotGroup[Find.Map.Size.x, Find.Map.Size.y, Find.Map.Size.z];
	}

	public void AddGroup(SlotGroup newGroup)
	{
		if (allGroups.Contains(newGroup))
		{
			Debug.LogError("Double-added slot group. Building is " + newGroup.building);
			return;
		}
		if (allGroups.Where((SlotGroup g) => g.building == newGroup.building).Any())
		{
			Debug.LogError("Added slot group with a building matching an existing one. Building is " + newGroup.building);
			return;
		}
		allGroups.Add(newGroup);
		foreach (IntVec3 item in newGroup)
		{
			groupGrid[item.x, item.y, item.z] = newGroup;
		}
	}

	public void RemoveGroup(SlotGroup oldGroup)
	{
		if (!allGroups.Contains(oldGroup))
		{
			Debug.LogError("Removing slot group that isn't registered.");
			return;
		}
		allGroups.Remove(oldGroup);
		foreach (IntVec3 item in oldGroup)
		{
			groupGrid[item.x, item.y, item.z] = null;
		}
	}

	public SlotGroup SlotGroupAt(IntVec3 loc)
	{
		return groupGrid[loc.x, loc.y, loc.z];
	}
}
