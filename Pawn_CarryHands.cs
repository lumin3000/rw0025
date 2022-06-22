using UnityEngine;

public class Pawn_CarryHands
{
	public const int CarryStackMaxSize = 75;

	private Pawn pawn;

	public Thing carriedThing;

	public Pawn_CarryHands(Pawn newPawn)
	{
		pawn = newPawn;
	}

	public void StartCarry(Thing newCar)
	{
		if (newCar == null)
		{
			Debug.LogError(string.Concat(this.pawn, " tried to StartCarry a null."));
			return;
		}
		if (newCar.carrier != null)
		{
			Debug.LogError(string.Concat(this.pawn, " tried to StartCarry ", newCar, " while ", newCar, " was already being carried by ", newCar.carrier, "."));
			return;
		}
		if (carriedThing != null)
		{
			if (!carriedThing.StacksWith(newCar))
			{
				Debug.LogError(string.Concat(this.pawn, " tried to start carrying ", newCar, " while already carrying ", carriedThing, "."));
				return;
			}
			int num = 75 - carriedThing.stackCount;
			if (num > newCar.stackCount)
			{
				num = newCar.stackCount;
			}
			carriedThing.stackCount += num;
			newCar.stackCount -= num;
			if (newCar.stackCount == 0)
			{
				newCar.Destroy();
			}
			return;
		}
		if (newCar.stackCount > 75)
		{
			StartCarry(newCar, 75);
			return;
		}
		if (newCar.HasAttachment(EntityType.Fire))
		{
			newCar.GetAttachment(EntityType.Fire).Destroy();
		}
		if (newCar.spawnedInWorld)
		{
			newCar.DeSpawn();
		}
		carriedThing = newCar;
		newCar.carrier = this.pawn;
		Find.Selector.Deselect(newCar);
		Pawn pawn = newCar as Pawn;
		if (pawn != null && !pawn.Incapacitated)
		{
			Find.ReservationManager.UnReserveAllForPawn(pawn);
			pawn.stances.CancelActionIfPossible();
			pawn.jobs.Notify_Carried();
			pawn.pather.StopDead();
			if (pawn.MindHuman != null && pawn.MindHuman.drafted)
			{
				pawn.MindHuman.drafted = false;
			}
		}
	}

	public void StartCarry(Thing newCar, int countToTake)
	{
		if (countToTake == 0)
		{
			Debug.LogError("Cannot start carrying 0 of something. Changing to 1.");
			countToTake = 1;
		}
		if (countToTake >= newCar.stackCount)
		{
			StartCarry(newCar);
			return;
		}
		Thing newCar2 = newCar.SplitOff(countToTake);
		StartCarry(newCar2);
	}

	public Thing DropCarriedThing()
	{
		if (carriedThing == null)
		{
			return null;
		}
		if (carriedThing.def.dropSound != null)
		{
			GenSound.PlaySoundAt(pawn.Position, carriedThing.def.dropSound, 0.2f);
		}
		IntVec3 newThingPos = ThingDropSpotFinder.BestDropSpotNear(pawn.Position);
		Thing result = ThingMaker.Spawn(carriedThing, newThingPos, carriedThing.rotation);
		if (carriedThing != null)
		{
			carriedThing.carrier = null;
		}
		carriedThing = null;
		return result;
	}

	public Thing DropCarriedThing(int Count)
	{
		if (Count == 0)
		{
			return null;
		}
		if (Count >= carriedThing.stackCount)
		{
			return DropCarriedThing();
		}
		Thing thing = carriedThing.SplitOff(Count);
		ThingMaker.Spawn(thing, pawn.Position, IntRot.random);
		thing.carrier = null;
		return thing;
	}

	public void DepositCarriedThingIntoResources()
	{
		if (carriedThing == null)
		{
			Debug.LogWarning(pawn.Label + " tried to deposit carried thing to resources without having a carried thing.");
			return;
		}
		ThingResource thingResource = (ThingResource)carriedThing;
		Find.ResourceManager.Gain(thingResource.def.eType, thingResource.stackCount);
		if (thingResource.def.dropSound != null)
		{
			GenSound.PlaySoundAt(pawn.Position, thingResource.def.dropSound, 0.2f);
		}
		carriedThing.Destroy();
	}

	public void CarryHandsTick()
	{
		if (carriedThing != null)
		{
			carriedThing.Tick();
		}
	}
}
