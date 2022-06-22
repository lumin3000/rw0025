using System.Collections.Generic;

public static class SkillDefinitions
{
	public static List<SkillDefinition> allSkills;

	static SkillDefinitions()
	{
		MakeSkillDefinitions();
	}

	public static SkillDefinition DefinitionOf(SkillType st)
	{
		return allSkills[(int)st];
	}

	public static SkillDefinition GetDefinition(this SkillType st)
	{
		return DefinitionOf(st);
	}

	private static void MakeSkillDefinitions()
	{
		allSkills = new List<SkillDefinition>();
		SkillDefinition skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Construction;
		skillDefinition.label = "Construction";
		skillDefinition.pawnLabel = "Constructor";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Growing;
		skillDefinition.label = "Growing";
		skillDefinition.pawnLabel = "Grower";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Research;
		skillDefinition.label = "Research";
		skillDefinition.pawnLabel = "Researcher";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Mining;
		skillDefinition.label = "Mining";
		skillDefinition.pawnLabel = "Miner";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Shooting;
		skillDefinition.label = "Shooting";
		skillDefinition.pawnLabel = "Shooter";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Melee;
		skillDefinition.label = "Melee";
		skillDefinition.pawnLabel = "Fighter";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Social;
		skillDefinition.label = "Social";
		skillDefinition.pawnLabel = "Socialite";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Cooking;
		skillDefinition.label = "Cooking";
		skillDefinition.pawnLabel = "Cook";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Medicine;
		skillDefinition.label = "Medicine";
		skillDefinition.pawnLabel = "Doctor";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Artistic;
		skillDefinition.label = "Artistic";
		skillDefinition.pawnLabel = "Artist";
		allSkills.Add(skillDefinition);
		skillDefinition = new SkillDefinition();
		skillDefinition.sType = SkillType.Crafting;
		skillDefinition.label = "Crafting";
		skillDefinition.pawnLabel = "Craftsman";
		allSkills.Add(skillDefinition);
	}

	public static string Label(this SkillType s)
	{
		return DefinitionOf(s).label;
	}
}
