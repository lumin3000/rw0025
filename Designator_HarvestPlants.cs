public class Designator_HarvestPlants : Designator_Plants
{
	public Designator_HarvestPlants()
	{
		buttonLabel = "Harvest plants";
		defaultDesc = "Marks plants to be harvested. Food will be harvested as possible from the plant, but the plant itself will remain.";
		buttonTexture = Res.LoadTexture("UI/DesignationIcons/HarvestPlants");
		dragStartClip = UISounds.Click;
		dragProgressClip = UISounds.DragLoopMeta;
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
		Plant plant = PlantUtility.PlantInSquare(sq);
		if (plant == null)
		{
			return false;
		}
		if (!plant.HarvestableNow)
		{
			return false;
		}
		if (Find.DesignationManager.DesignationAt(sq, DesignationType.HarvestPlant) != null)
		{
			return false;
		}
		return true;
	}

	public override void DesignateAt(IntVec3 sq)
	{
		Find.DesignationManager.AddDesignation(new Designation_HarvestPlant(PlantUtility.PlantInSquare(sq)));
	}

	public override void FinalizeDesignationFailed()
	{
		UI_Messages.Message("Must click on edible plants that are grown enough to harvest.", UIMessageSound.Reject);
	}
}
