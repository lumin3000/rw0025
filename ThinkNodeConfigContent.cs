using UnityEngine;

public static class ThinkNodeConfigContent
{
	public static ThinkNode GetNewNodesForConfigFor(ThinkNodeConfig config, Pawn pawn, Pawn_Mind mind)
	{
		switch (config)
		{
		case ThinkNodeConfig.HumanStandard:
			return ThinkNodeTreesHardcoded.NewNodes_Human(pawn, (Pawn_MindHuman)mind);
		case ThinkNodeConfig.Herbivore:
			return ThinkNodeTreesHardcoded.NewNodes_Herbivore(pawn);
		case ThinkNodeConfig.HerbivoreHerd:
			return ThinkNodeTreesHardcoded.NewNodes_HerbivoreHerd(pawn);
		default:
			Debug.LogError("No ThinkNodeConfig available for " + config);
			return null;
		}
	}
}
