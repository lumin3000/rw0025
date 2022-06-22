using System.Collections.Generic;

public class PawnKindDefinition
{
	public string raceName = string.Empty;

	public string kindLabel = "UndefinedKindLabel";

	public TeamType defaultTeam;

	public ThinkNodeConfig thinkConfig;

	public PawnSetupMethod setupMethod;

	public bool aiAvoidCover;

	public float recruitmentLoyaltyThreshold = 50f;

	public PathingParameters pathParams = PathParameters.smart;

	public float baseIncapChancePerDamage = 0.01f;

	public CharHistoryCategory historyCategory;

	public List<string> bodyGraphicNames = new List<string>();

	public bool wildSpawn_spawnWild;

	public float wildSpawn_EcoSystemWeight = 1f;

	public IntRange wildSpawn_GroupSizeRange = IntRange.one;

	public float wildSpawn_SelectionWeight = 1f;

	public RaceDefinition RaceDef => RaceDefDatabase.DefinitionNamed(raceName);

	public bool UseStandardGraphics => bodyGraphicNames.Count > 0;
}
