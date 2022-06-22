using System.Text;
using UnityEngine;

public class Skill
{
	public const int MinLevel = 0;

	public const int MaxLevel = 20;

	private Pawn pawn;

	public SkillType sType;

	public int level;

	public float xpSinceLastLevel;

	public float XpRequiredForLevelUp => XpRequiredToLevelUpFrom(level);

	public string Label => sType.GetDefinition().label;

	public float XpProgressPercent => xpSinceLastLevel / XpRequiredForLevelUp;

	public float XpTotalEarned
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < level; i++)
			{
				num += XpRequiredToLevelUpFrom(i);
			}
			return num;
		}
	}

	public string LevelString => level.ToString();

	public bool Disabled
	{
		get
		{
			foreach (WorkType disabledWork in pawn.story.DisabledWorks)
			{
				foreach (SkillType relevantSkill in disabledWork.GetDefinition().relevantSkills)
				{
					if (relevantSkill == sType)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public Skill(Pawn pawn, SkillType newSType)
	{
		this.pawn = pawn;
		sType = newSType;
	}

	public float XpRequiredToLevelUpFrom(int startingLevel)
	{
		return 1000 + startingLevel * 1000;
	}

	public void Learn(float gainedXp)
	{
		if (level < 20)
		{
			if (DebugSettings.fastLearning)
			{
				gainedXp *= 100f;
			}
			xpSinceLastLevel += gainedXp * 1.1f;
			while (xpSinceLastLevel >= XpRequiredForLevelUp)
			{
				xpSinceLastLevel -= XpRequiredForLevelUp;
				level++;
			}
		}
	}

	public TooltipDef GetTooltip()
	{
		string empty = string.Empty;
		string text = empty;
		empty = text + "Progress to next level: " + xpSinceLastLevel.ToString("########0") + " / " + XpRequiredForLevelUp;
		empty += "\n\n";
		empty += GetSkillDescription();
		return new TooltipDef(empty, (int)sType * 397945);
	}

	private string GetSkillDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (sType == SkillType.Mining)
		{
			return "Mines at " + ((0.5f + 0.15f * (float)level) * 100f).ToString("###0") + "% speed.";
		}
		if (sType == SkillType.Research)
		{
			return "Researches at " + ((0.1f + 0.15f * (float)level) * 100f).ToString("###0") + "% speed.";
		}
		if (sType == SkillType.Growing)
		{
			return "Sows and harvests at " + ((0.2f + 0.12f * (float)level) * 100f).ToString("###0") + "% speed.";
		}
		if (sType == SkillType.Construction)
		{
			return "Constructs and repairs at " + ((0.5f + 0.15f * (float)level) * 100f).ToString("###0") + "% speed.";
		}
		if (sType == SkillType.Social)
		{
			stringBuilder.Append("Social interactions are " + (10f * (float)level).ToString("###0") + "% more impactful.");
			stringBuilder.AppendLine();
			stringBuilder.Append("Recruits easy prisoners " + (0.05f * (float)level * 100f).ToString("###0") + "% of the time.");
			float num = 0.5f * (float)level;
			stringBuilder.AppendLine();
			stringBuilder.Append("Non-commodity buy and sell prices are " + num.ToString("###0") + "% better when negotiating.");
			return stringBuilder.ToString();
		}
		if (sType == SkillType.Shooting)
		{
			return "Miss chance per square of distance is " + ((1f - SkillTunings.AccuracyAtLevel[level]) * 100f).ToString("###0.00") + "%.";
		}
		if (sType == SkillType.Melee)
		{
			return "Melee attack base hit chance is " + (SkillTunings.MeleeHitChanceAtLevel[level] * 100f).ToString("###0") + "%.";
		}
		if (sType == SkillType.Cooking)
		{
			return "This skill does nothing in this unfinished version of RimWorld.";
		}
		if (sType == SkillType.Medicine)
		{
			return "This skill does nothing in this unfinished version of RimWorld.";
		}
		if (sType == SkillType.Artistic)
		{
			return "This skill does nothing in this unfinished version of RimWorld.";
		}
		if (sType == SkillType.Crafting)
		{
			return "This skill does nothing in this unfinished version of RimWorld.";
		}
		Debug.Log(string.Concat("Skill ", sType, " needs bonus description."));
		return "Error.";
	}
}
