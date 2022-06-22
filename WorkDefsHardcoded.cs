using System.Collections.Generic;

public static class WorkDefsHardcoded
{
	public static IEnumerable<WorkDefinition> AllWorkDefinitions()
	{
		yield return new WorkDefinition
		{
			wType = WorkType.NoWork,
			pawnLabel = "Off-duty",
			gerundLabel = "NoWorking",
			automatic = false
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Soldier,
			pawnLabel = "Soldier",
			gerundLabel = "Soldiering",
			automatic = false,
			tooltipDesc = "Soldiers take orders and fight. Unlike other works, they do not take initiative. They only go exactly where you tell them.Soldiers will never take breaks to eat or rest. You must undraft them sometimes.",
			relevantSkills = 
			{
				SkillType.Melee,
				SkillType.Shooting
			},
			workTags = WorkTags.Violent
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Research,
			pawnLabel = "Researcher",
			gerundLabel = "Researching",
			tooltipDesc = "Researchers do research work.",
			verb = "Research at",
			naturalPriority = 10,
			relevantSkills = { SkillType.Research },
			workTags = WorkTags.Intellectual
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Cleaning,
			pawnLabel = "Cleaner",
			gerundLabel = "Cleaning",
			tooltipDesc = "Cleaners wipe up spills and sweep up debris in designated home zones, making the colony more enjoyable to live in.",
			verb = "Clean",
			naturalPriority = 11,
			startActive = true,
			workTags = WorkTags.ManualDumb
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Hauling,
			pawnLabel = "Hauler",
			gerundLabel = "Hauling",
			tooltipDesc = "Haulers carry things to where they need to be.",
			verb = "Haul",
			naturalPriority = 12,
			startActive = true,
			workTags = WorkTags.ManualDumb
		};
		yield return new WorkDefinition
		{
			wType = WorkType.PlantCutting,
			pawnLabel = "Plant cutter",
			gerundLabel = "Plant cutting",
			tooltipDesc = "Plant cutters remove and harvest the wild plants you've designated.",
			verb = "Cut",
			naturalPriority = 13,
			startActive = true,
			workTags = WorkTags.ManualDumb
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Mining,
			pawnLabel = "Miner",
			gerundLabel = "Mining",
			tooltipDesc = "Miners dig out squares that you have designated for mining. If these are mineral squares, they will yield usable minerals.",
			verb = "Mine",
			naturalPriority = 14,
			startActive = true,
			relevantSkills = { SkillType.Mining },
			workTags = WorkTags.ManualSkilled
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Growing,
			pawnLabel = "Grower",
			gerundLabel = "Growing",
			tooltipDesc = "Growers plant seeds and harvest grown crops.",
			verb = "Grow",
			naturalPriority = 15,
			startActive = true,
			relevantSkills = { SkillType.Growing },
			workTags = WorkTags.ManualSkilled
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Repair,
			pawnLabel = "Repairer",
			gerundLabel = "Repairing",
			tooltipDesc = "Repairers fix broken-down buildings and equipment.",
			verb = "Repair",
			naturalPriority = 16,
			startActive = true,
			relevantSkills = { SkillType.Construction },
			workTags = WorkTags.ManualSkilled
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Construction,
			pawnLabel = "Constructor",
			gerundLabel = "Constructing",
			tooltipDesc = "Constructors build things from blueprints and repair damaged buildings.",
			verb = "Construct",
			naturalPriority = 17,
			startActive = true,
			relevantSkills = { SkillType.Construction },
			workTags = WorkTags.ManualSkilled
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Warden,
			pawnLabel = "Warden",
			gerundLabel = "Warden",
			tooltipDesc = "Wardens visit prisoners to recruit them, have a nice chat, beat them, or execute them - whatever you have decided.Wardens also deliver food to prisoners.",
			verb = "Handle",
			naturalPriority = 18,
			relevantSkills = { SkillType.Social },
			workTags = WorkTags.Social
		};
		yield return new WorkDefinition
		{
			wType = WorkType.Doctor,
			pawnLabel = "Doctor",
			gerundLabel = "Doctoring",
			tooltipDesc = "Doctors bring food those lying incapacitated in bed. They will care for both colonists and prisoners.",
			verb = "Care for",
			naturalPriority = 20,
			workTags = WorkTags.Caring
		};
		WorkDefinition d = new WorkDefinition();
		d.wType = WorkType.Firefighter;
		d.pawnLabel = "Firefighter";
		d.gerundLabel = "Firefighting";
		d.tooltipDesc = "Firefighters extinguish fires in the colony.";
		d.verb = "Extinguish";
		d.naturalPriority = 30;
		d.workTags = WorkTags.None;
		d.startActive = true;
		d.emergency = true;
		d.workTags = WorkTags.Scary | WorkTags.Firefighting;
		yield return d;
	}
}
