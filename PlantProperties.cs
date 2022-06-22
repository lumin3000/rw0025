public class PlantProperties
{
	public float maxFoodYield;

	public float rotDamagePerTick;

	public IntRange generateClusterSizeRange = IntRange.one;

	public int maxMeshCount = 1;

	public FloatRange sizeRange = new FloatRange(0.9f, 1.1f);

	public float topWindExposure = 0.25f;

	public bool destroyOnHarvest;

	public bool wild;

	public float wildCommonality = 1f;

	public float growthPer20kTicks = 0.25f;

	public PsychGlow minGlowToGrow = PsychGlow.Overlit;

	public float minFertility = 0.9f;

	public float fertilityFactorGrowthRate = 0.5f;

	public float fertilityFactorPlantChance;

	public int lifeSpan = -1;

	public ThingDefinition seedDefinition;

	public float SeedShootMinGrowthPercent = 0.5f;

	public float SeedEmitAveragePer20kTicks;

	public float SeedShootRadius = 6f;

	public bool Harvestable => maxFoodYield > 0f;

	public bool LimitedLifespan => lifeSpan > 0;
}
