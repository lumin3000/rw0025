public class Designator_CutPlants : Designator_Plants
{
	public Designator_CutPlants()
	{
		buttonLabel = "Cut plants";
		defaultDesc = "Marks plants to be cut and destroyed. The plant will be completely removed, and any food will be harvested.";
		buttonTexture = Res.LoadTexture("UI/DesignationIcons/CutPlants");
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
		if (PlantUtility.PlantInSquare(sq) == null)
		{
			return false;
		}
		if (Find.DesignationManager.DesignationAt(sq, DesignationType.CutPlant) != null)
		{
			return false;
		}
		return true;
	}

	public override void DesignateAt(IntVec3 sq)
	{
		Find.DesignationManager.AddDesignation(new Designation_CutPlant(PlantUtility.PlantInSquare(sq)));
	}

	public override void FinalizeDesignationFailed()
	{
		UI_Messages.Message("Must click on plants.", UIMessageSound.Reject);
	}
}
