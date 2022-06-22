using System.Linq;

public class Designator_Cancel : Designator
{
	public Designator_Cancel()
	{
		buttonLabel = "Cancel";
		defaultDesc = "Cancel designated building, mining, hauling, harvesting, and all other designations.";
		buttonTexture = Res.LoadTexture("UI/DesignationIcons/Cancel");
		dragVolumeMax = 0.5f;
		useMouseIcon = true;
	}

	public override AcceptanceReport CanDesignateAt(IntVec3 sq)
	{
		AcceptanceReport acceptanceReport = base.CanDesignateAt(sq);
		if (!acceptanceReport.accepted)
		{
			return acceptanceReport;
		}
		if (Find.DesignationManager.AllDesignationsAt(sq).Count() > 0)
		{
			return AcceptanceReport.WasAccepted;
		}
		Thing thing = Find.Grids.ThingAt(sq, EntityType.Blueprint);
		if (thing != null && thing.Team == TeamType.Colonist)
		{
			return AcceptanceReport.WasAccepted;
		}
		Thing thing2 = Find.Grids.ThingAt(sq, EntityType.BuildingFrame);
		if (thing2 != null && thing2.Team == TeamType.Colonist)
		{
			return AcceptanceReport.WasAccepted;
		}
		return AcceptanceReport.WasRejected;
	}

	public override void DesignateAt(IntVec3 loc)
	{
		foreach (Designation item in Find.DesignationManager.AllDesignationsAt(loc).ToList())
		{
			Find.DesignationManager.RemoveDesignation(item);
		}
		Blueprint blueprint = (Blueprint)Find.Grids.ThingAt(loc, EntityType.Blueprint);
		if (blueprint != null && blueprint.Team == TeamType.Colonist)
		{
			blueprint.CancelBlueprint();
		}
		BuildingFrame buildingFrame = (BuildingFrame)Find.Grids.ThingAt(loc, EntityType.BuildingFrame);
		if (buildingFrame != null && buildingFrame.Team == TeamType.Colonist)
		{
			GenMap.ReclaimResourcesFor(buildingFrame);
			buildingFrame.Destroy();
		}
	}

	public override void FinalizeDesignationSucceeded()
	{
		GenSound.PlaySoundOnCamera("Interface/DesignateCancel", 0.15f);
	}

	public override void DesignatorUpdate()
	{
		GenUI.RenderMouseoverBracket();
	}
}
