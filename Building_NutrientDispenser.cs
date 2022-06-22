using UnityEngine;

public class Building_NutrientDispenser : Building
{
	private const int FoodCostPerDispense = 10;

	private CompPowerTrader powerComp;

	private static readonly AudioClip SoundDispense = Res.LoadSound("Interaction/Food/Dispense/DispensePaste");

	public static int CollectDuration = 50;

	public bool CanDispenseNow => powerComp.PowerOn && Find.ResourceManager.Food >= 10 && this.HasAir();

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		powerComp = GetComp<CompPowerTrader>();
	}

	public Thing DispenseFood()
	{
		if (!CanDispenseNow)
		{
			return null;
		}
		int num = 10;
		if (Find.ResearchManager.HasResearched(ResearchType.NutrientResynthesis))
		{
			num--;
		}
		Find.ResourceManager.Food -= num;
		GenSound.PlaySoundAt(base.Position, SoundDispense, 0.15f);
		return (Meal)ThingMaker.MakeThing("MealNutrientPaste");
	}
}
