using System.Collections.Generic;

public class JobDriver_FoodDeliver : JobDriverToil
{
	protected Pawn Deliveree => (Pawn)base.CurJob.targetB.thing;

	public JobDriver_FoodDeliver(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Delivering food to " + Deliveree.characterName + ".", JobReportOverlays.feeder);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil(delegate
		{
			Find.ReservationManager.ReserveFor(pawn, Deliveree, ReservationType.DeliverFood);
		}, ToilCompleteMode.Immediate);
		yield return Toils_Food.GotoFoodSource(pawn, base.TargetThingA);
		yield return Toils_Food.PickupFoodFromSource(pawn, base.TargetThingA);
		yield return new Toil(delegate
		{
			Room roomAt = Find.Grids.GetRoomAt(Deliveree.Position);
			IntVec3 intVec = roomAt.squaresList.RandomElement();
			pawn.pather.StartPathTowards(intVec, autoReserve: false, adjacentIsOK: true);
		}, ToilCompleteMode.PatherArrival)
		{
			tickFailCondition = delegate
			{
				if (Deliveree.Incapacitated)
				{
					return true;
				}
				if (!ToilTools.CanInteractStandard(pawn, Deliveree))
				{
					return true;
				}
				return (!Deliveree.prisoner.getsFood) ? true : false;
			}
		};
		yield return new Toil(delegate
		{
			pawn.carryHands.DropCarriedThing();
		}, ToilCompleteMode.Immediate);
	}
}
