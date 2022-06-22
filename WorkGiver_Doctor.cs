using System.Collections;

public class WorkGiver_Doctor : WorkGiver
{
	public override IEnumerable PotentialWorkTargets => Find.PawnManager.ColonistsAndPrisoners;

	public WorkGiver_Doctor(Pawn newPawn)
		: base(newPawn)
	{
		wType = WorkType.Doctor;
	}

	public override Job StartingJobOn(Thing t)
	{
		Pawn pawn = t as Pawn;
		if (pawn == null)
		{
			return null;
		}
		if (t.Team != TeamType.Colonist && t.Team != TeamType.Prisoner)
		{
			return null;
		}
		if (!base.pawn.CanReserve(t, ReservationType.DeliverFood))
		{
			return null;
		}
		Pawn pawn2 = (Pawn)t;
		if (!pawn2.food.Food.ShouldTrySatisfy || !pawn2.Incapacitated)
		{
			return null;
		}
		Thing thing = FoodUtility.FindFoodSourceFor(base.pawn);
		if (thing != null)
		{
			Job job = new Job(JobType.FeedPatient);
			job.targetA = new TargetPack(thing);
			job.targetB = new TargetPack(pawn2);
			return job;
		}
		return null;
	}
}
