public static class FoodUtility
{
	public static float NutritionAvailableFromFor(Thing t, Pawn p)
	{
		if (t.def.Edible && p.raceDef.CanEat(t))
		{
			return t.def.food.nutrition;
		}
		if (p.raceDef.CanUseTechnology)
		{
			Building_NutrientDispenser building_NutrientDispenser = t as Building_NutrientDispenser;
			if (building_NutrientDispenser != null && building_NutrientDispenser.CanDispenseNow)
			{
				return 99999f;
			}
		}
		return 0f;
	}

	public static Thing FindFoodSourceFor(Pawn p)
	{
		Thing thing = null;
		Thing thing2 = null;
		GenScan.CloseToThingValidator validator = delegate(Thing t)
		{
			if (!p.raceDef.CanEat(t.def.food.category))
			{
				return false;
			}
			if (p.Team == TeamType.Colonist && t.IsForbidden())
			{
				return false;
			}
			if (!((Edible)t).EdibleNow)
			{
				return false;
			}
			if (!t.IsSociallyProperForUseBy(p))
			{
				return false;
			}
			if (!p.raceDef.BreathesOutside && !t.HasAir())
			{
				return false;
			}
			if (!p.AwareOf(t))
			{
				return false;
			}
			return p.CanReserve(t, ReservationType.Total) ? true : false;
		};
		thing = GenScan.ClosestReachableThing(p.Position, Find.Map.thingLister.spawnedEdibles, validator);
		if (p.raceDef.CanUseTechnology && Find.ResourceManager.Food > 0)
		{
			GenScan.CloseToThingValidator validator2 = delegate(Thing t)
			{
				if (!t.IsSociallyProperForUseBy(p))
				{
					return false;
				}
				if (!p.raceDef.BreathesOutside && !t.HasAir())
				{
					return false;
				}
				Building_NutrientDispenser building_NutrientDispenser = (Building_NutrientDispenser)t;
				if (!building_NutrientDispenser.InteractionSquare.Standable())
				{
					return false;
				}
				return (p.Team != TeamType.Colonist || !t.IsForbidden()) && building_NutrientDispenser.CanDispenseNow;
			};
			thing2 = GenScan.ClosestReachableThing(p.Position, Find.BuildingManager.AllBuildingsColonistOfClass<Building_NutrientDispenser>(), validator2);
		}
		if (thing2 != null && thing != null)
		{
			if (thing.def.food.category < FoodCategory.Prepared)
			{
				return thing2;
			}
			if ((thing2.Position - p.Position).LengthHorizontalSquared < (thing.Position - p.Position).LengthHorizontalSquared)
			{
				return thing2;
			}
			return thing;
		}
		if (thing2 != null)
		{
			return thing2;
		}
		if (thing != null)
		{
			return thing;
		}
		return null;
	}
}
