using UnityEngine;

public static class ThingCompMaker
{
	public static ThingComp MakeThingComp(CompSetup setup)
	{
		switch (setup)
		{
		case CompSetup.Explosive_19Flame:
		{
			CompExplosive compExplosive5 = new CompExplosive();
			compExplosive5.explosiveRadius = 1.9f;
			compExplosive5.explosiveDamType = DamageType.Flame;
			compExplosive5.setup = setup;
			return compExplosive5;
		}
		case CompSetup.Explosive_FallenGrenade:
		{
			CompExplosive compExplosive4 = new CompExplosive();
			compExplosive4.explosiveRadius = 1.9f;
			compExplosive4.explosiveDamType = DamageType.Bomb;
			compExplosive4.wickTicksMin = 125;
			compExplosive4.wickTicksMax = 150;
			compExplosive4.wickScale = 0.9f;
			compExplosive4.setup = setup;
			return compExplosive4;
		}
		case CompSetup.Explosive_29:
		{
			CompExplosive compExplosive3 = new CompExplosive();
			compExplosive3.explosiveRadius = 2.9f;
			compExplosive3.explosiveDamType = DamageType.Bomb;
			compExplosive3.setup = setup;
			return compExplosive3;
		}
		case CompSetup.Explosive_39:
		{
			CompExplosive compExplosive2 = new CompExplosive();
			compExplosive2.explosiveRadius = 3.9f;
			compExplosive2.explosiveDamType = DamageType.Bomb;
			compExplosive2.setup = setup;
			return compExplosive2;
		}
		case CompSetup.Explosive_49:
		{
			CompExplosive compExplosive = new CompExplosive();
			compExplosive.explosiveRadius = 4.9f;
			compExplosive.explosiveDamType = DamageType.Bomb;
			compExplosive.setup = setup;
			return compExplosive;
		}
		case CompSetup.PowerTrader:
		{
			CompPowerTrader compPowerTrader = new CompPowerTrader();
			compPowerTrader.setup = setup;
			return compPowerTrader;
		}
		case CompSetup.PowerBattery:
		{
			CompPowerBattery compPowerBattery = new CompPowerBattery();
			compPowerBattery.storedEnergyMax = 1000f;
			compPowerBattery.efficiency = 0.5f;
			compPowerBattery.setup = setup;
			return compPowerBattery;
		}
		case CompSetup.Glower_Medium:
		{
			CompGlower compGlower2 = new CompGlower();
			compGlower2.setup = setup;
			compGlower2.glowRadius = 12f;
			compGlower2.glowColor = new ColorInt(255, 255, 255, 0) * 0.85f;
			return compGlower2;
		}
		case CompSetup.Glower_Overlight:
		{
			CompGlower compGlower = new CompGlower();
			compGlower.setup = setup;
			compGlower.canOverLight = true;
			compGlower.glowRadius = 14f;
			compGlower.glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;
			return compGlower;
		}
		case CompSetup.TouchTrigger_Trap:
		{
			CompTouchTrigger compTouchTrigger = new CompTouchTrigger();
			compTouchTrigger.TouchHostile = true;
			compTouchTrigger.TouchNeutral = false;
			compTouchTrigger.TouchPlayer = false;
			compTouchTrigger.setup = setup;
			return compTouchTrigger;
		}
		case CompSetup.Forbiddable:
			return new CompForbiddable();
		default:
			Debug.LogError("Missing CompSetup " + setup);
			return null;
		}
	}
}
