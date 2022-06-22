using System;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_SkillsTracker : Saveable
{
	private Pawn pawn;

	public List<Skill> AllSkills = new List<Skill>();

	public Pawn_SkillsTracker(Pawn newPawn)
	{
		pawn = newPawn;
		foreach (int value in Enum.GetValues(typeof(SkillType)))
		{
			Skill item = new Skill(pawn, (SkillType)value);
			AllSkills.Add(item);
		}
	}

	public void ExposeData()
	{
		foreach (Skill allSkill in AllSkills)
		{
			Scribe.LookField(ref allSkill.level, "Level_" + allSkill.sType, 1, forceSave: true);
			Scribe.LookField(ref allSkill.xpSinceLastLevel, "XpSinceLastLevel_" + allSkill.sType, 0f, forceSave: true);
		}
	}

	public Skill SkillOfType(SkillType sType)
	{
		foreach (Skill allSkill in AllSkills)
		{
			if (allSkill.sType == sType)
			{
				return allSkill;
			}
		}
		Debug.LogError("Did not find skill of type " + sType);
		return null;
	}

	public void Learn(SkillType sType, float xp)
	{
		SkillOfType(sType).Learn(xp);
	}

	public int LevelOf(SkillType sType)
	{
		return SkillOfType(sType).level;
	}

	public void SetLevel(SkillType sType, int NewLevel)
	{
		SkillOfType(sType).level = NewLevel;
	}
}
