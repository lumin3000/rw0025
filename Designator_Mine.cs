public class Designator_Mine : Designator
{
	public Designator_Mine()
	{
		buttonLabel = "Mine";
		buttonTexture = Res.LoadTexture("UI/DesignationIcons/Mine");
		defaultDesc = "Designate areas of rock to be mined out.";
		dragStartClip = UISounds.Click;
		dragProgressClip = UISounds.DragLoopMeta;
		dragVolumeMax = 0.5f;
		useMouseIcon = true;
	}

	public override AcceptanceReport CanDesignateAt(IntVec3 loc)
	{
		AcceptanceReport acceptanceReport = base.CanDesignateAt(loc);
		if (!acceptanceReport.accepted)
		{
			return acceptanceReport;
		}
		if (Find.DesignationManager.DesignationAt(loc, DesignationType.Mine) != null)
		{
			return AcceptanceReport.WasRejected;
		}
		if (loc.IsFogged())
		{
			return true;
		}
		if (MineUtility.MineableInSquare(loc) == null)
		{
			return AcceptanceReport.WasRejected;
		}
		return AcceptanceReport.WasAccepted;
	}

	public override void DesignateAt(IntVec3 loc)
	{
		Find.DesignationManager.AddDesignation(new Designation_Mine(loc));
	}

	public override void FinalizeDesignationSucceeded()
	{
		GenSound.PlaySoundOnCamera("Interface/DesignateMine", 0.15f);
	}

	public override void FinalizeDesignationFailed()
	{
	}

	public override void DesignatorUpdate()
	{
		GenUI.RenderMouseoverBracket();
	}
}
