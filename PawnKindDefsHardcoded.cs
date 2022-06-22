using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class PawnKindDefsHardcoded
{
	public static IEnumerable<PawnKindDefinition> AllPawnKindDefsHardcoded()
	{
		MethodInfo[] methods = typeof(PawnKindDefsHardcoded).GetMethods();
		foreach (MethodInfo method in methods)
		{
			if (!method.Name.StartsWith("Definitions_"))
			{
				continue;
			}
			foreach (PawnKindDefinition item in (IEnumerable)method.Invoke(null, null))
			{
				yield return item;
			}
		}
	}

	public static IEnumerable<PawnKindDefinition> Definitions_Animals()
	{
		yield return new PawnKindDefinition
		{
			kindLabel = "Muffalo",
			raceName = "Muffalo",
			pathParams = PathParameters.animal,
			thinkConfig = ThinkNodeConfig.HerbivoreHerd,
			bodyGraphicNames = { "Muffalo" },
			wildSpawn_spawnWild = true,
			wildSpawn_EcoSystemWeight = 1f,
			wildSpawn_GroupSizeRange = new IntRange(3, 9),
			wildSpawn_SelectionWeight = 0.5f
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Squirrel",
			raceName = "Squirrel",
			pathParams = PathParameters.animal,
			thinkConfig = ThinkNodeConfig.Herbivore,
			bodyGraphicNames = { "Squirrel" },
			wildSpawn_EcoSystemWeight = 0.2f,
			wildSpawn_spawnWild = true,
			wildSpawn_SelectionWeight = 1f
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Boomrat",
			raceName = "Boomrat",
			pathParams = PathParameters.animal,
			thinkConfig = ThinkNodeConfig.Herbivore,
			bodyGraphicNames = { "Boomrat" },
			wildSpawn_EcoSystemWeight = 0.2f,
			wildSpawn_spawnWild = true,
			wildSpawn_SelectionWeight = 1f
		};
	}

	public static IEnumerable<PawnKindDefinition> Definitions_Civil()
	{
		yield return new PawnKindDefinition
		{
			kindLabel = "Colonist",
			raceName = "Human",
			defaultTeam = TeamType.Colonist,
			thinkConfig = ThinkNodeConfig.HumanStandard,
			baseIncapChancePerDamage = 0.13f,
			recruitmentLoyaltyThreshold = 60f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Civil,
			setupMethod = delegate
			{
			}
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Slave",
			raceName = "Human",
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 60f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Slave,
			setupMethod = delegate
			{
			}
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Refugee",
			raceName = "Human",
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 30f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Civil,
			setupMethod = delegate(Pawn pawn)
			{
				pawn.health = Random.Range(55, 65);
			}
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Traveler",
			raceName = "Human",
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 60f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Civil,
			setupMethod = delegate
			{
			}
		};
	}

	public static IEnumerable<PawnKindDefinition> Definitions_Raiders()
	{
		yield return new PawnKindDefinition
		{
			kindLabel = "Drifter",
			raceName = "Human",
			defaultTeam = TeamType.Raider,
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 35f,
			pathParams = PathParameters.stupid,
			historyCategory = CharHistoryCategory.Pirate,
			setupMethod = delegate(Pawn pawn)
			{
				pawn.health = Random.Range(55, 65);
				pawn.equipment.MakeAndAddEquipment("Gun_Pistol");
			}
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Scavenger",
			raceName = "Human",
			defaultTeam = TeamType.Raider,
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 60f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Pirate,
			setupMethod = delegate(Pawn pawn)
			{
				pawn.health = Random.Range(75, 90);
				float value3 = Random.value;
				if (value3 < 0.8f)
				{
					pawn.equipment.MakeAndAddEquipment("Gun_Pistol");
				}
				else if (value3 < 0.9f)
				{
					pawn.equipment.MakeAndAddEquipment("Gun_Lee-Enfield");
				}
				else
				{
					pawn.equipment.MakeAndAddEquipment("Gun_Pump Shotgun");
				}
			}
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Pirate",
			raceName = "Human",
			defaultTeam = TeamType.Raider,
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 70f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Pirate,
			setupMethod = delegate(Pawn pawn)
			{
				float value2 = Random.value;
				if (value2 < 0.25f)
				{
					pawn.equipment.MakeAndAddEquipment("Gun_Lee-Enfield");
				}
				else if (value2 < 0.5f)
				{
					pawn.equipment.MakeAndAddEquipment("Gun_Uzi");
				}
				else if (value2 < 0.75f)
				{
					pawn.equipment.MakeAndAddEquipment("Gun_Pump Shotgun");
				}
				else
				{
					pawn.equipment.MakeAndAddEquipment("Gun_T-9 Incendiary Launcher");
				}
			}
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Sniper",
			raceName = "Human",
			defaultTeam = TeamType.Raider,
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 70f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Pirate,
			setupMethod = delegate(Pawn pawn)
			{
				pawn.equipment.MakeAndAddEquipment("Gun_M-24Rifle");
			}
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Mercenary",
			raceName = "Human",
			defaultTeam = TeamType.Raider,
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 80f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Pirate,
			setupMethod = delegate(Pawn pawn)
			{
				float value = Random.value;
				if (value < 0.25f)
				{
					pawn.equipment.MakeAndAddEquipment("Gun_M-16Rifle");
				}
				else if (value < 0.5f)
				{
					pawn.equipment.MakeAndAddEquipment("Gun_M-16Rifle");
				}
				else if (value < 0.75f)
				{
					pawn.equipment.MakeAndAddEquipment("Gun_R-4 charge rifle");
				}
				else
				{
					pawn.equipment.MakeAndAddEquipment("Gun_M-24Rifle");
				}
			}
		};
		yield return new PawnKindDefinition
		{
			kindLabel = "Grenadier",
			raceName = "Human",
			defaultTeam = TeamType.Raider,
			thinkConfig = ThinkNodeConfig.HumanStandard,
			recruitmentLoyaltyThreshold = 70f,
			pathParams = PathParameters.smart,
			historyCategory = CharHistoryCategory.Pirate,
			setupMethod = delegate(Pawn pawn)
			{
				if (Random.value < 0.5f)
				{
					pawn.equipment.MakeAndAddEquipment("Weapon_GrenadeFrag");
				}
				else
				{
					pawn.equipment.MakeAndAddEquipment("Weapon_GrenadeMolotov");
				}
			}
		};
	}

	public static IEnumerable<PawnKindDefinition> Definitions_Robots()
	{
		yield return new PawnKindDefinition
		{
			kindLabel = "Gunbot",
			raceName = "Robot",
			defaultTeam = TeamType.Raider,
			thinkConfig = ThinkNodeConfig.RobotCombat,
			pathParams = PathParameters.robot,
			historyCategory = CharHistoryCategory.Robot,
			aiAvoidCover = true,
			setupMethod = delegate(Pawn pawn)
			{
				pawn.skills.SetLevel(SkillType.Shooting, 7);
				pawn.equipment.MakeAndAddEquipment("Gun_Minigun");
			}
		};
	}
}
