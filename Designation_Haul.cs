public class Designation_Haul : Designation
{
	public Designation_Haul()
	{
		dType = DesignationType.Haul;
		iconMat = MaterialPool.MatFrom("UI/Overlays/Designations/Haul", MatBases.MetaOverlay);
	}

	public Designation_Haul(Thing targetThing)
		: this()
	{
		target = new TargetPack(targetThing);
	}

	public override void DesignationDraw()
	{
		if (target.thing.carrier == null)
		{
			base.DesignationDraw();
		}
	}
}
