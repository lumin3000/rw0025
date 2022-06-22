using System;
using UnityEngine;

public class RaceDefinition
{
	public const float DefaultPawnMoveTicks = 14f;

	public string raceName = "UndefinedRaceLabel";

	public NameType nameType;

	public int baseMaxHealth;

	public float baseMoveTicks_Cardinal;

	public float baseMoveTicks_Diagonal;

	public int meleeDamage = 10;

	public bool canBleed = true;

	public bool hasGenders = true;

	public bool hasIdentity;

	public bool humanoid;

	public float targetHitEase = 1f;

	public FoodCategory foodRequirement;

	public float hungerThreshold;

	public bool needsRest = true;

	public Action<Pawn> deathAction;

	public bool pawnOverdraw;

	public Mesh shadowMesh;

	public bool UsesEquipment => humanoid;

	public bool GloballyAware => humanoid;

	public bool CanUseTechnology => humanoid;

	public bool BreathesOutside => !humanoid;

	public bool EatsFood => foodRequirement != FoodCategory.NoFood;

	public bool MakesFootprints => humanoid;

	public void SetMovementTicksFromWalkSpeed(float walkSpeed)
	{
		float num = 1f / walkSpeed;
		baseMoveTicks_Cardinal = (int)Math.Round(14f * num);
		baseMoveTicks_Diagonal = (int)Math.Round(14f * num * 1.41f);
	}

	public bool CanEat(FoodCategory foodCat)
	{
		if (foodRequirement == FoodCategory.NoFood)
		{
			return false;
		}
		return foodRequirement <= foodCat;
	}

	public bool CanEat(Thing t)
	{
		return CanEat(t.def.food.category);
	}
}
