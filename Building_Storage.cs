using System.Collections.Generic;

public class Building_Storage : Building
{
	public SlotGroup slotGroup;

	public Building_Storage()
	{
		slotGroup = new SlotGroup(this);
	}

	public virtual void Notify_ReceivedThing(Thing newItem)
	{
	}

	public virtual void Notify_LosingThing(Thing newItem)
	{
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		slotGroup.Notify_BuildingSpawned();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookSaveable(ref slotGroup, "SlotGroup", this);
	}

	public override void Destroy()
	{
		slotGroup.Notify_BuildingDestroyed();
		base.Destroy();
	}

	public override IEnumerable<Command> GetCommandOptions()
	{
		foreach (Command slotConfigCommand in slotGroup.GetSlotConfigCommands())
		{
			yield return slotConfigCommand;
		}
	}
}
