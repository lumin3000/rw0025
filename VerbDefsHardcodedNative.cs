using System.Collections.Generic;

public static class VerbDefsHardcodedNative
{
	public static IEnumerable<VerbDefinition> AllVerbDefinitions()
	{
		yield return new VerbDefinition
		{
			id = VerbID.Melee,
			label = "Melee",
			description = "Melee attack.",
			warmupTicks = 0,
			cooldownTicks = 100,
			range = 1f,
			noiseRadius = 3f,
			targetParams = 
			{
				targetTeams = TargetingParameters.AllTeams,
				canTargetPawns = true,
				canTargetBuildings = true,
				worldObjectTargetsMustBeAutoAttackable = true
			}
		};
		yield return new VerbDefinition
		{
			id = VerbID.BeatFire,
			label = "Beat fire",
			description = "Beat at flames to extinguish them.",
			range = 1f,
			noiseRadius = 3f,
			targetParams = 
			{
				canTargetFires = true
			},
			warmupTicks = 0,
			cooldownTicks = 65
		};
		yield return new VerbDefinition
		{
			id = VerbID.Ignite,
			label = "Ignite",
			description = "Start something on fire.",
			range = 1f,
			noiseRadius = 3f,
			targetParams = 
			{
				onlyTargetFlammables = true,
				canTargetBuildings = true
			},
			warmupTicks = 80,
			cooldownTicks = 80
		};
	}
}
