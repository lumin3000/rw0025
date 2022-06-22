public static class SkillTunings
{
	public const float LearnRateGlobalMultiplier = 1.1f;

	public const float XpPerPickHitMining = 13f;

	public const float MiningSpeedBase = 0.5f;

	public const float MiningSpeedPctPerLevel = 0.15f;

	public const float XpPerResearchTick = 0.22f;

	public const float ResearchSpeedBase = 0.1f;

	public const float ResearchSpeedPctPerLevel = 0.15f;

	public const float XpPerTickConstruction = 0.7f;

	public const float ConstructionSpeedBase = 0.5f;

	public const float ConstructionSpeedPctPerLevel = 0.15f;

	public const float XpPerPlanting = 25f;

	public const float XpPerSowGrowing = 25f;

	public const float XpPerHarvestGrowing = 50f;

	public const float GrowingSpeedBase = 0.2f;

	public const float GrowingSpeedPctBonusPerLevel = 0.12f;

	public const float XpPerBurstFiredPractice = 10f;

	public const float XpPerBurstFiredNeutralPawn = 50f;

	public const float XpPerBurstFiredInAnger = 200f;

	public const float XpPerMeleeHit = 100f;

	public const float XpPerIncidentalSocialInteraction = 4f;

	public const float XpPerPrisonerChat = 50f;

	public const float SocialEffectBonusPctPerLevel = 0.1f;

	public const float RecruitChancePerLevel = 0.05f;

	public const float TradeablePriceImprovementPerLevel = 0.005f;

	public static readonly float[] AccuracyAtLevel = new float[21]
	{
		0.7f, 0.85f, 0.93f, 0.95f, 0.95f, 0.955f, 0.96f, 0.965f, 0.97f, 0.975f,
		0.98f, 0.985f, 0.9875f, 0.99f, 0.991f, 0.992f, 0.993f, 0.994f, 0.995f, 0.996f,
		0.997f
	};

	public static readonly float[] MeleeHitChanceAtLevel = new float[21]
	{
		0.3f, 0.4f, 0.5f, 0.55f, 0.6f, 0.625f, 0.65f, 0.675f, 0.7f, 0.725f,
		0.75f, 0.775f, 0.8f, 0.825f, 0.85f, 0.875f, 0.9f, 0.92f, 0.94f, 0.96f,
		0.98f
	};
}
