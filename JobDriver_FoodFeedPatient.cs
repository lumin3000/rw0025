using System;
using System.Collections.Generic;

public class JobDriver_FoodFeedPatient : JobDriverToil
{
	private const float FeedDurationMultiplier = 1.5f;

	protected Pawn Deliveree => (Pawn)base.CurJob.targetB.thing;

	public JobDriver_FoodFeedPatient(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Delivering food to " + Deliveree.characterName + ".", JobReportOverlays.feeder);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		List<Toil> list = new List<Toil>();
		Func<bool> tickFailCondition = delegate
		{
			if (!Deliveree.Incapacitated)
			{
				return true;
			}
			if (!ToilTools.CanInteractStandard(pawn, Deliveree))
			{
				return true;
			}
			return (Deliveree.Team == TeamType.Prisoner && !Deliveree.prisoner.getsFood) ? true : false;
		};
		Toil item = new Toil(delegate
		{
			Find.ReservationManager.ReserveFor(pawn, Deliveree, ReservationType.DeliverFood);
		}, ToilCompleteMode.Immediate);
		list.Add(item);
		list.Add(Toils_Food.GotoFoodSource(pawn, base.TargetThingA));
		list.Add(Toils_Food.PickupFoodFromSource(pawn, base.TargetThingA));
		Toil toil = new Toil();
		toil.initAction = delegate
		{
			pawn.pather.StartPathTowards(Deliveree);
		};
		toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
		toil.tickFailCondition = tickFailCondition;
		list.Add(toil);
		Toil toil2 = Toils_Food.ChewFood(pawn, Deliveree, 1.5f);
		toil2.tickFailCondition = tickFailCondition;
		list.Add(toil2);
		list.Add(Toils_Food.FinishChewing(pawn, Deliveree));
		return list;
	}
}
