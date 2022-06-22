using System.Collections.Generic;

public static class RaceDefsHardcoded
{
	private static RaceDefinition BaseAnimalDefinition()
	{
		RaceDefinition raceDefinition = new RaceDefinition();
		raceDefinition.hasIdentity = false;
		raceDefinition.needsRest = false;
		raceDefinition.foodRequirement = FoodCategory.Unharvested;
		raceDefinition.canBleed = true;
		raceDefinition.hasGenders = true;
		return raceDefinition;
	}

	public static IEnumerable<RaceDefinition> Definitions_Animal()
	{
		RaceDefinition r3 = BaseAnimalDefinition();
		r3.raceName = "Muffalo";
		r3.baseMaxHealth = 160;
		r3.SetMovementTicksFromWalkSpeed(0.5f);
		r3.meleeDamage = 10;
		r3.pawnOverdraw = true;
		r3.shadowMesh = MeshPool.shadow0606;
		r3.hungerThreshold = 80f;
		r3.targetHitEase = 2f;
		yield return r3;
		r3 = BaseAnimalDefinition();
		r3.raceName = "Squirrel";
		r3.baseMaxHealth = 30;
		r3.SetMovementTicksFromWalkSpeed(1.5f);
		r3.meleeDamage = 3;
		r3.pawnOverdraw = false;
		r3.hungerThreshold = 80f;
		r3.targetHitEase = 0.6f;
		yield return r3;
		r3 = BaseAnimalDefinition();
		r3.raceName = "Boomrat";
		r3.baseMaxHealth = 50;
		r3.SetMovementTicksFromWalkSpeed(1.5f);
		r3.meleeDamage = 5;
		r3.pawnOverdraw = false;
		r3.hungerThreshold = 80f;
		r3.deathAction = delegate(Pawn p)
		{
			Explosion.DoExplosion(p.Position, 1.9f, DamageType.Flame);
		};
		r3.targetHitEase = 0.6f;
		yield return r3;
	}

	public static IEnumerable<RaceDefinition> Definitions_Humanoid()
	{
		RaceDefinition r = new RaceDefinition();
		r.raceName = "Human";
		r.baseMaxHealth = 100;
		r.humanoid = true;
		r.SetMovementTicksFromWalkSpeed(1f);
		r.canBleed = true;
		r.meleeDamage = 10;
		r.nameType = NameType.HumanStandard;
		r.hasIdentity = true;
		r.shadowMesh = MeshPool.shadow0408;
		r.foodRequirement = FoodCategory.Harvested;
		r.hungerThreshold = 30f;
		yield return r;
	}

	public static IEnumerable<RaceDefinition> Definitions_Robot()
	{
		RaceDefinition r = new RaceDefinition();
		r.raceName = "Robot";
		r.baseMaxHealth = 350;
		r.SetMovementTicksFromWalkSpeed(0.25f);
		r.canBleed = false;
		r.meleeDamage = 20;
		r.nameType = NameType.Robot;
		r.hasGenders = false;
		r.hasIdentity = true;
		r.needsRest = false;
		r.pawnOverdraw = true;
		r.shadowMesh = MeshPool.shadow0608;
		yield return r;
	}
}
