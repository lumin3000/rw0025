using UnityEngine;

public class BreakdownManager
{
	private const float CheckInterval = 99f;

	private const float BreakdownChanceOverall = 0.02f;

	private const float WearRandomChancePerBuilding = 0.004f;

	private const float RainFireRandomChancePerBuilding = 0.2f;

	public void BreakdownManagerTick()
	{
		if ((float)Find.TickManager.tickCount % 99f == 0f)
		{
			if (Random.value < 0.02f)
			{
				RollForWear();
			}
			if (Random.value < 0.02f)
			{
				RollForRainFire();
			}
		}
	}

	private void RollForWear()
	{
		float num = 0.004f * (float)Find.BuildingManager.AllBuildingsColonist.Count;
		if (!(Random.value > num))
		{
			Building building = Find.BuildingManager.RandomBuildingPlayer();
			if (building.def.eType != EntityType.BuildingFrame)
			{
				int newAmount = Mathf.CeilToInt((float)building.health * Random.Range(0.3f, 0.4f));
				building.TakeDamage(new DamageInfo(DamageType.Breakdown, newAmount));
			}
		}
	}

	private void RollForRainFire()
	{
		float num = 0.2f * (float)Find.BuildingManager.AllBuildingsColonistElecFire.Count * Find.WeatherManager.RainRate;
		if (Random.value > num)
		{
			return;
		}
		Building building = Find.BuildingManager.AllBuildingsColonistElecFire.RandomElement();
		if (!Find.RoofGrid.Roofed(building.Position))
		{
			ThingWithComponents thingWithComponents = building;
			if ((thingWithComponents.GetComp<CompPowerTrader>() != null && thingWithComponents.GetComp<CompPowerTrader>().PowerOn) || (thingWithComponents.GetComp<CompPowerBattery>() != null && thingWithComponents.GetComp<CompPowerBattery>().storedEnergy > 100f))
			{
				Explosion.DoExplosion(Gen.SquaresOccupiedBy(building).RandomElement(), 1.9f, DamageType.Flame);
				Find.LetterStack.ReceiveLetter(new Letter("A " + building.Label.ToLower() + " has short-circuited in the rain and started a fire.", building.Position));
			}
		}
	}
}
