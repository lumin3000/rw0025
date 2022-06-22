using System.Collections.Generic;
using System.Linq;

public class JobDriver_Haul : JobDriverToil
{
	protected const float DupeScanRadius = 8f;

	private ThingResource TargetResource => (ThingResource)base.TargetThingA;

	private IntVec3 DestLoc => base.CurJob.targetB.Loc;

	public JobDriver_Haul(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		Thing thing = null;
		thing = ((pawn.carryHands.carriedThing == null) ? base.TargetThingA : pawn.carryHands.carriedThing);
		string text = "error";
		if (base.JType == JobType.HaulToSlot)
		{
			text = DestLoc.ContainingSlotGroup().building.Label;
		}
		if (base.JType == JobType.HaulToCargo)
		{
			text = "cargo";
		}
		return new JobReport("Hauling " + thing.Label + " to " + text + ".", JobReportOverlays.hauler);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil
		{
			initAction = delegate
			{
				Find.PawnDestinationManager.UnreserveAllFor(pawn);
				Find.ReservationManager.ReserveFor(pawn, base.TargetThingA, ReservationType.Total);
				if (base.JType == JobType.HaulToSlot)
				{
					Find.ReservationManager.ReserveFor(pawn, DestLoc, ReservationType.Store);
				}
				Toils_General.GetToPickup(pawn, base.TargetThingA);
			},
			defaultCompleteMode = ToilCompleteMode.PatherArrival,
			tickFailCondition = delegate
			{
				if (base.JType == JobType.HaulToSlot && !DestLoc.IsValidStorageFor(base.TargetThingA))
				{
					return true;
				}
				if (base.TargetThingA.def.designateHaulable && Find.DesignationManager.DesignationOn(base.TargetThingA, DesignationType.Haul) == null && Find.Grids.ThingAt(base.TargetThingA.Position, EntityType.Blueprint) == null)
				{
					return true;
				}
				if (base.TargetThingA.destroyed)
				{
					return true;
				}
				if (base.TargetThingA.IsForbidden())
				{
					Blueprint blueprint = Find.Grids.ThingAt<Blueprint>(base.TargetThingA.Position);
					if (blueprint == null || blueprint.SmallThingBlockingPlacement != base.TargetThingA)
					{
						return true;
					}
				}
				return false;
			}
		};
		yield return new Toil
		{
			initAction = delegate
			{
				Find.ReservationManager.UnReserve(base.TargetThingA, ReservationType.Total);
				pawn.carryHands.StartCarry(base.TargetThingA);
				if (base.TargetThingA.def.pickupSoundFolder != null)
				{
					GenSound.PlaySoundAt(pawn.Position, GenSound.RandomClipInFolder(base.TargetThingA.def.pickupSoundFolder), 0.17f);
				}
			},
			defaultCompleteMode = ToilCompleteMode.Immediate
		};
		yield return new Toil
		{
			initAction = delegate
			{
				Toil gotoDest;
				if (base.JType == JobType.HaulToCargo)
				{
					ThingResource thingResource = FindNearbyDuplicateResource();
					if (thingResource != null)
					{
						base.TargetThingA = thingResource;
						Toil GetHaulTarget;
						SetNextToil(GetHaulTarget);
						SetCompleteMode(ToilCompleteMode.Immediate);
					}
					else
					{
						base.TargetThingA = pawn.carryHands.carriedThing;
						Building building = (Building)GenScan.ClosestReachableThing(pawn.Position, Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Area_Stockpile));
						gotoDest.tickFailCondition = ToilTools.StandardTickFail(pawn, building);
						if ((from sq in Gen.SquaresOccupiedBy(building)
							where sq == pawn.Position
							select sq).Any())
						{
							SetCompleteMode(ToilCompleteMode.Immediate);
						}
						else
						{
							pawn.pather.StartPathTowards(building);
						}
					}
				}
				else if (base.JType == JobType.HaulToSlot)
				{
					if (!DestLoc.IsValidStorageFor(base.TargetThingA))
					{
						EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						TargetPack newDest = ((!DestLoc.Walkable()) ? ((TargetPack)DestLoc.ContainingSlotGroup().building) : ((TargetPack)DestLoc));
						gotoDest.tickFailCondition = () => !DestLoc.IsValidStorageFor(base.TargetThingA);
						pawn.pather.StartPathTowards(newDest);
					}
				}
			},
			defaultCompleteMode = ToilCompleteMode.PatherArrival
		};
		yield return new Toil(delegate
		{
			if (base.JType == JobType.HaulToCargo)
			{
				pawn.carryHands.DepositCarriedThingIntoResources();
			}
			if (base.JType == JobType.HaulToSlot)
			{
				Thing thing = pawn.carryHands.DropCarriedThing();
				thing.Position = DestLoc;
				Find.DesignationManager.RemoveAllDesignationsOn(base.TargetThingA);
				SlotGroup slotGroup = DestLoc.ContainingSlotGroup();
				if (slotGroup.building != null)
				{
					slotGroup.building.Notify_ReceivedThing(thing);
				}
			}
		}, ToilCompleteMode.Immediate);
	}

	private ThingResource FindNearbyDuplicateResource()
	{
		if (75 - pawn.carryHands.carriedThing.stackCount == 0)
		{
			return null;
		}
		GenScan.CloseToThingValidator validator = delegate(Thing t)
		{
			if (t == base.TargetThingA || t == pawn.carryHands.carriedThing)
			{
				return false;
			}
			if (!t.StacksWith(pawn.carryHands.carriedThing))
			{
				return false;
			}
			if (t.IsForbidden())
			{
				return false;
			}
			return pawn.CanReserve(t, ReservationType.Total) ? true : false;
		};
		return (ThingResource)GenScan.ClosestReachableThing(pawn.Position, Find.Map.thingLister.spawnedHaulables, 8f, validator);
	}
}
