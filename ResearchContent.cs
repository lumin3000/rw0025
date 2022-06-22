using System.Collections.Generic;

public static class ResearchContent
{
	public static List<ResearchProject> DefaultProjectList
	{
		get
		{
			List<ResearchProject> list = new List<ResearchProject>();
			ResearchProject item = new ResearchProject(ResearchType.Hydroponics, 22000f, "Hydroponics", "Allows you to build hydroponics tables to rapidly grow crops indoors.");
			list.Add(item);
			item = new ResearchProject(ResearchType.PneumaticPicks, 30000f, "Pneumatic picks", "Miner's picks are 20% more effective.");
			list.Add(item);
			item = new ResearchProject(ResearchType.NutrientResynthesis, 22000f, "Nutrient resynthesis", "Nutrient paste dispensers consume 10% less food per meal produced.");
			list.Add(item);
			item = new ResearchProject(ResearchType.GunTurretCooling, 35000f, "Gun turret cooling", "Gun turrets fire four shots in a burst instead of three.");
			list.Add(item);
			item = new ResearchProject(ResearchType.BlastingCharges, 15000f, "Blasting charges", "Allows building Blasting Charges, triggered explosives made to quickly reduce rock to rubble.");
			list.Add(item);
			item = new ResearchProject(ResearchType.CarpetMaking, 15000f, "Carpet making", "Allows colonists to build carpets to increase the quality of their environments.");
			list.Add(item);
			item = new ResearchProject(ResearchType.FearTech1, 9000f, "Fear tech 1", "Research new ways to inspire fear.", "You can now build gibbet cages to display the corpses of dead enemies and colonist troublemakers. Those that look upon them will feel intense fear and disgust. This is an ancient, crude, but and powerful device.");
			list.Add(item);
			return list;
		}
	}
}
