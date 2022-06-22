using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Toils_Food
{
	public static Toil GotoFoodSource(Pawn pawn, Thing foodSource)
	{
		Toil toil = new Toil(delegate
		{
			Building_NutrientDispenser building_NutrientDispenser = foodSource as Building_NutrientDispenser;
			if (building_NutrientDispenser != null)
			{
				pawn.pather.StartPathTowards(building_NutrientDispenser.InteractionSquare);
			}
			else
			{
				Find.ReservationManager.ReserveFor(pawn, foodSource, ReservationType.Total);
				pawn.pather.StartPathTowards(foodSource.Position, autoReserve: false, adjacentIsOK: true);
			}
		}, ToilCompleteMode.PatherArrival);
		toil.tickFailCondition = ToilTools.StandardTickFail(pawn, foodSource);
		return toil;
	}

	public static Toil PickupFoodFromSource(Pawn pawn, Thing foodSource)
	{
		Toil toil = new Toil(delegate
		{
			Building_NutrientDispenser building_NutrientDispenser = foodSource as Building_NutrientDispenser;
			if (building_NutrientDispenser != null)
			{
				Thing thing = (Meal)building_NutrientDispenser.DispenseFood();
				if (thing == null)
				{
					pawn.jobs.CurJobDriver.EndJobWith(JobCondition.Incompletable);
					return;
				}
				pawn.carryHands.StartCarry(thing);
			}
			else
			{
				int countToTake = foodSource.stackCount;
				if (foodSource.stackCount > 0)
				{
					countToTake = Mathf.CeilToInt(pawn.food.NutritionWanted / foodSource.def.food.nutrition);
				}
				pawn.carryHands.StartCarry(foodSource, countToTake);
			}
			pawn.jobs.CurJob.targetA = pawn.carryHands.carriedThing;
		}, ToilCompleteMode.Delay);
		toil.duration = Building_NutrientDispenser.CollectDuration;
		return toil;
	}

	public static Toil CarryFoodToChewSpot(Pawn carrier)
	{
		Toil toil = new Toil(delegate
		{
			GenScan.CloseToThingValidator validator = delegate(Thing t)
			{
				if (!carrier.CanReserve(t, ReservationType.UseDevice))
				{
					return false;
				}
				if (!carrier.raceDef.BreathesOutside && !t.Position.HasAir())
				{
					return false;
				}
				return t.IsSociallyProperForUseBy(carrier) ? true : false;
			};
			Building_Chair building_Chair = (Building_Chair)GenScan.ClosestReachableThing(carrier.Position, Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Building_Chair), 20f, validator);
			IntVec3 intVec;
			if (building_Chair != null)
			{
				intVec = building_Chair.Position;
				Find.ReservationManager.ReserveFor(carrier, building_Chair, ReservationType.UseDevice);
			}
			else
			{
				Predicate<IntVec3> validator2 = delegate(IntVec3 sq)
				{
					if (!sq.Standable() || !sq.HasAir() || Find.PawnDestinationManager.DestinationIsReserved(sq) || Find.Grids.GetRoomAt(carrier.Position) != Find.Grids.GetRoomAt(sq) || Find.Grids.SquareContains(sq, EntityType.Door))
					{
						return false;
					}
					foreach (Thing item in Find.Grids.ThingsAt(sq))
					{
						if (item.def.pathCost > 10)
						{
							return false;
						}
					}
					return true;
				};
				intVec = GenMap.RandomMapSquareNear(carrier.Position, 8, validator2, out var succeeded);
				if (!succeeded)
				{
					Debug.LogWarning(string.Concat(carrier, " could not find eat spot. Had to eat in place at ", carrier.Position, "."));
					intVec = carrier.Position;
				}
			}
			Find.PawnDestinationManager.ReserveDestinationFor(carrier, intVec);
			carrier.pather.StartPathTowards(intVec);
		}, ToilCompleteMode.PatherArrival);
		toil.tickFailCondition = null;
		return toil;
	}

	public static Toil PlaceFoodForEating(Pawn carrier)
	{
		return new Toil(delegate
		{
			Building_Chair building_Chair = Find.Grids.ThingAt<Building_Chair>(carrier.Position);
			Thing carriedThing = carrier.carryHands.carriedThing;
			if (!(carriedThing is Edible))
			{
				Debug.Log(string.Concat(carrier, " tried to place food for eating but was carrying ", carrier.carryHands.carriedThing));
				carrier.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			else
			{
				carrier.carryHands.DropCarriedThing();
				if (Find.ReservationManager.ReserverOf(carriedThing) != carrier)
				{
					Find.ReservationManager.ReserveFor(carrier, carriedThing, ReservationType.Total);
				}
				if (building_Chair != null && building_Chair.IsFacingTable)
				{
					carriedThing.Position = building_Chair.SpotInFrontOfChair;
					if (carriedThing.def.rotatable)
					{
						carriedThing.rotation = building_Chair.rotation;
					}
				}
				else
				{
					List<IntVec3> list = Gen.CardinalDirections.InRandomOrder().Concat(Gen.CornerDirections.InRandomOrder()).ToList();
					list.Add(IntVec3.zero);
					foreach (IntVec3 item in list)
					{
						IntVec3 intVec = carrier.Position + item;
						if (intVec.Walkable())
						{
							carriedThing.Position = intVec;
							if (carriedThing.def.rotatable && item != IntVec3.zero)
							{
								carriedThing.rotation = IntRot.FromIntVec3(item);
							}
							break;
						}
					}
				}
			}
		}, ToilCompleteMode.Immediate);
	}

	public static Toil ChewFood(Pawn pawn, Pawn chewer, float durationMultiplier)
	{
		EffectMaker_EatStandard chewEffect = new EffectMaker_EatStandard();
		Toil chewFood = new Toil();
		chewFood.initAction = delegate
		{
			Thing thing = pawn.jobs.CurJob.targetA;
			if (!(thing is Edible))
			{
				Debug.Log(string.Concat(chewer, " tried to chew ", thing, " which is not an Edible."));
				chewer.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			else
			{
				chewFood.duration = Mathf.RoundToInt((float)thing.def.food.baseEatTicks * durationMultiplier);
				if (thing.def.food.eatSoundList.Count > 0 && chewer.raceDef.humanoid)
				{
					GenSound.PlaySoundAt(chewer.Position, thing.def.food.eatSoundList.RandomElement(), 0.5f);
				}
			}
		};
		chewFood.defaultCompleteMode = ToilCompleteMode.Delay;
		chewFood.tickFailCondition = ToilTools.StandardTickFail(pawn, pawn.jobs.CurJob.targetA);
		chewFood.tickAction = delegate
		{
			chewEffect.EffectTick(pawn, pawn.jobs.CurJob.targetA);
			if (chewer.talker != null && UnityEngine.Random.value < 0.01f)
			{
				chewer.talker.TryIncidentalSocialSpeech();
			}
		};
		return chewFood;
	}

	public static Toil FinishChewing(Pawn driverPawn, Pawn eatingPawn)
	{
		return new Toil(delegate
		{
			Thing eatenThing = driverPawn.jobs.CurJob.targetA;
			eatingPawn.food.Food.Notify_ThingEaten(eatenThing);
		}, ToilCompleteMode.Immediate);
	}
}
