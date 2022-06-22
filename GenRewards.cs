using System.Collections.Generic;
using UnityEngine;

public static class GenRewards
{
	public static List<Tradeable> GenerateRewards(RewardCode RCode)
	{
		return RCode switch
		{
			RewardCode.Undefined => null, 
			RewardCode.SmallResources => GenerateReward_SmallResources(), 
			RewardCode.StandardResources => GenerateReward_StandardResources(), 
			RewardCode.StandardRandom => GenerateReward_StandardRandom(), 
			RewardCode.Food => GenerateReward_StandardRandom(), 
			RewardCode.Medicine => GenerateReward_StandardRandom(), 
			RewardCode.Weapon => GenerateReward_StandardRandom(), 
			_ => null, 
		};
	}

	public static List<Tradeable> GenerateReward_StandardRandom()
	{
		return GenerateReward_StandardResources();
	}

	public static List<Tradeable> GenerateReward_StandardResources()
	{
		if (Random.value < 0.5f)
		{
			return GenerateReward_Food();
		}
		return GenerateReward_Medicine();
	}

	private static List<Tradeable> GenerateReward_Food()
	{
		return new List<Tradeable>();
	}

	private static List<Tradeable> GenerateReward_Medicine()
	{
		return new List<Tradeable>();
	}

	private static List<Tradeable> GenerateReward_SmallResources()
	{
		return new List<Tradeable>();
	}
}
