using System.Collections.Generic;

public class JobDriver_FoodEat : JobDriverToil
{
	public JobDriver_FoodEat(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Eating from " + base.TargetThingA.Label + ".", JobReportOverlays.eater);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return Toils_Food.GotoFoodSource(pawn, base.TargetThingA);
		if (pawn.raceDef.CanUseTechnology)
		{
			yield return Toils_Food.PickupFoodFromSource(pawn, base.TargetThingA);
			yield return Toils_Food.CarryFoodToChewSpot(pawn);
			yield return Toils_Food.PlaceFoodForEating(pawn);
		}
		yield return Toils_Food.ChewFood(pawn, pawn, 1f);
		yield return Toils_Food.FinishChewing(pawn, pawn);
	}
}
