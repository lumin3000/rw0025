public class Designator_Clean : Designator
{
	private bool shouldClean;

	public Designator_Clean(bool shouldClean)
	{
		this.shouldClean = shouldClean;
		if (shouldClean)
		{
			buttonLabel = "Add Home Zone";
			defaultDesc = "Colonists will clean and extinguish fires in the home zone.";
			buttonTexture = Res.LoadTexture("UI/DesignationIcons/HomeZoneOn");
		}
		else
		{
			buttonLabel = "Remove Home Zone";
			defaultDesc = "Removes a home zone. Colonists will no longer clean or extinguish fires here.";
			buttonTexture = Res.LoadTexture("UI/DesignationIcons/HomeZoneOff");
		}
		dragStartClip = UISounds.Click;
		dragProgressClip = UISounds.DragLoopMeta;
		dragVolumeMax = 0.5f;
		useMouseIcon = true;
	}

	public override AcceptanceReport CanDesignateAt(IntVec3 sq)
	{
		return sq.InBounds() && Find.HomeZoneGrid.ShouldClean(sq) != shouldClean;
	}

	public override void DesignateAt(IntVec3 sq)
	{
		Find.HomeZoneGrid.SetShouldClean(sq, shouldClean);
	}

	public override void FinalizeDesignationSucceeded()
	{
		GenSound.PlaySoundOnCamera("Interface/DesignateMine", 0.15f);
	}

	public override void DesignatorUpdate()
	{
		GenUI.RenderMouseoverBracket();
		OverlayDrawHandler.DrawHomeZoneOverlay();
	}
}
