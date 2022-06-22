public static class ThingDefBases
{
	public static ThingDefinition NewBaseDefinitionFrom(EntityType newType)
	{
		ThingDefinition thingDefinition = new ThingDefinition();
		thingDefinition.eType = newType;
		switch (newType)
		{
		case EntityType.Proj_Seed:
			thingDefinition = ThingDefsHardcoded.NewBaseProjectileDefinition();
			thingDefinition.label = "Unspecified seed";
			thingDefinition.thingClass = typeof(Seed);
			thingDefinition.texturePath = "Icons/Plant/Seed_Default";
			thingDefinition.category = EntityCategory.Projectile;
			thingDefinition.useStandardHealth = true;
			thingDefinition.selectable = true;
			thingDefinition.maxHealth = 20;
			thingDefinition.altitudeLayer = AltitudeLayer.Projectile;
			thingDefinition.desc = "Seed lacks desc.";
			return thingDefinition;
		case EntityType.Blueprint:
			thingDefinition.label = "Unspecified blueprint";
			thingDefinition.thingClass = typeof(Blueprint);
			thingDefinition.category = EntityCategory.Special;
			thingDefinition.altitudeLayer = AltitudeLayer.Blueprint;
			thingDefinition.useStandardHealth = false;
			thingDefinition.hasTooltip = true;
			thingDefinition.tickerType = TickerType.Normal;
			thingDefinition.selectable = true;
			thingDefinition.seeThroughFog = true;
			return thingDefinition;
		case EntityType.BuildingFrame:
			thingDefinition.label = "Unspecified building frame";
			thingDefinition.thingClass = typeof(BuildingFrame);
			thingDefinition.category = EntityCategory.Special;
			thingDefinition.altitudeLayer = AltitudeLayer.Waist;
			thingDefinition.useStandardHealth = true;
			thingDefinition.hasTooltip = true;
			thingDefinition.selectable = true;
			thingDefinition.beauty = BeautyCategory.Ugly;
			return thingDefinition;
		default:
			return null;
		}
	}
}
