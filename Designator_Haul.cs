public class Designator_Haul : Designator
{
	public Designator_Haul()
	{
		buttonLabel = "Haul things";
		buttonTexture = Res.LoadTexture("UI/DesignationIcons/Haul");
		defaultDesc = "Mark debris and other items to be hauled to dumping areas. Not needed for things like food or corpses, which are always haulable.";
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
		Thing thing = HaulUtility.HaulableInSquare(sq);
		if (thing == null || (thing.IsInStorage() && thing.StorageIsValid()))
		{
			return AcceptanceReport.WasRejected;
		}
		if (Find.DesignationManager.DesignationAt(sq, DesignationType.Haul) != null)
		{
			return false;
		}
		return AcceptanceReport.WasAccepted;
	}

	public override void DesignateAt(IntVec3 Loc)
	{
		Find.DesignationManager.AddDesignation(new Designation_Haul(HaulUtility.HaulableInSquare(Loc)));
	}

	public override void FinalizeDesignationSucceeded()
	{
		GenSound.PlaySoundOnCamera("Interface/DesignateMine", 0.15f);
	}

	public override void FinalizeDesignationFailed()
	{
		UI_Messages.Message("Must haul debris.", UIMessageSound.Reject);
	}

	public override void DesignatorUpdate()
	{
		GenUI.RenderMouseoverBracket();
	}
}
