using System.Collections;
using UnityEngine;

public class WorkGiver_Warden : WorkGiver
{
	private const int PrisonerBeatingNumAttacks = 5;

	public override IEnumerable PotentialWorkTargets => Find.PawnManager.PawnsOnTeam[TeamType.Prisoner];

	public WorkGiver_Warden(Pawn newPawn)
		: base(newPawn)
	{
		wType = WorkType.Warden;
	}

	public override Job StartingJobOn(Thing t)
	{
		if (t.Team != TeamType.Prisoner)
		{
			return null;
		}
		Pawn pawn = t as Pawn;
		if (pawn == null)
		{
			return null;
		}
		if (pawn.carrier != null)
		{
			return null;
		}
		if (!pawn.Incapacitated && base.pawn.CanReserve(pawn, ReservationType.Total))
		{
			bool flag = pawn.ownership.ownedBed != null && Find.Grids.GetRoomAt(pawn.ownership.ownedBed.Position) != Find.Grids.GetRoomAt(pawn.Position);
			bool flag2 = false;
			Room room = pawn.ContainingRoom();
			if (room != null)
			{
				foreach (Building_Bed containedBed in room.ContainedBeds)
				{
					if (containedBed.forPrisoners && (containedBed.owner == null || containedBed.owner == pawn))
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag || !flag2)
			{
				Building_Bed building_Bed = BedUtility.FindBedFor(pawn, TeamType.Prisoner, checkSocialProperness: false);
				if (building_Bed != null)
				{
					return new Job(JobType.EscortPrisonerToBed, pawn, building_Bed);
				}
			}
		}
		if (pawn.Incapacitated && !pawn.IsInBed() && base.pawn.CanReserve(pawn, ReservationType.Total) && BedUtility.FindBedFor(pawn) != null)
		{
			return new Job(JobType.TakeWoundedPrisonerToBed, pawn, BedUtility.FindBedFor(pawn));
		}
		if (pawn.prisoner.getsFood && pawn.food.Food.ShouldTrySatisfy)
		{
			Thing thing = FoodUtility.FindFoodSourceFor(base.pawn);
			if (thing != null)
			{
				if (pawn.Incapacitated)
				{
					if (base.pawn.CanReserve(t, ReservationType.DeliverFood))
					{
						return new Job(JobType.FeedPatient, thing, pawn);
					}
				}
				else
				{
					bool flag3 = false;
					if (pawn.carryHands.carriedThing != null && FoodUtility.NutritionAvailableFromFor(pawn.carryHands.carriedThing, pawn) > 0f)
					{
						flag3 = true;
					}
					if (!flag3)
					{
						float num = 0f;
						float num2 = 0f;
						Room roomAt = Find.Grids.GetRoomAt(pawn.Position);
						if (roomAt != null)
						{
							foreach (Thing allContainedThing in roomAt.AllContainedThings)
							{
								float num3 = FoodUtility.NutritionAvailableFromFor(allContainedThing, pawn);
								num2 += num3;
								Pawn pawn2 = allContainedThing as Pawn;
								if (pawn2 != null && pawn2.Team == TeamType.Prisoner && pawn2.food.Food.ShouldTrySatisfy && (pawn2.carryHands.carriedThing == null || !pawn2.raceDef.CanEat(pawn2.carryHands.carriedThing)))
								{
									num += pawn2.food.NutritionWanted;
								}
							}
						}
						if (num2 >= num + 40f)
						{
							flag3 = true;
						}
					}
					if (!flag3 && base.pawn.CanReserve(t, ReservationType.DeliverFood))
					{
						return new Job(JobType.DeliverFood, thing, pawn);
					}
				}
			}
		}
		if (pawn.prisoner.ScheduledForInteraction && base.pawn.CanReserve(t, ReservationType.HumanInteraction))
		{
			PrisonerInteractionMode interactionMode = pawn.prisoner.interactionMode;
			bool flag4 = (interactionMode == PrisonerInteractionMode.FriendlyChat || interactionMode == PrisonerInteractionMode.BeatingMild) && pawn.IsInBed();
			if ((interactionMode == PrisonerInteractionMode.Execution || !pawn.Incapacitated) && !flag4)
			{
				switch (pawn.prisoner.interactionMode)
				{
				case PrisonerInteractionMode.BeatingMild:
				{
					Job job2 = new Job(JobType.PrisonerBeatingMild, t);
					job2.maxNumMeleeAttacks = 5;
					return job2;
				}
				case PrisonerInteractionMode.BeatingVicious:
				{
					Job job = new Job(JobType.PrisonerBeatingVicious, t);
					job.maxNumMeleeAttacks = 5;
					return job;
				}
				case PrisonerInteractionMode.FriendlyChat:
					return new Job(JobType.PrisonerFriendlyChat, t);
				case PrisonerInteractionMode.Execution:
					return new Job(JobType.PrisonerExecution, t);
				default:
					Debug.LogError("Work warden failed to give prisoner job.");
					return null;
				}
			}
		}
		return null;
	}
}
