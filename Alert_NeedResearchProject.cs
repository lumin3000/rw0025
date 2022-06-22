public class Alert_NeedResearchProject : Alert
{
	public override AlertReport Report => Find.ResearchManager.CurrentProj == null && Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_ResearchBench) && Find.ResearchManager.ProjectsAreAvailable;

	public Alert_NeedResearchProject()
	{
		basePriority = AlertPriority.Medium;
		baseLabel = "Need research project";
		baseExplanation = "You have the equipment to do research but have not selected a project.\n\nOpen the research menu and select a project.";
	}
}
