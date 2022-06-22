public class Designation_CutPlant : Designation
{
	public Designation_CutPlant()
	{
		dType = DesignationType.CutPlant;
		iconMat = MaterialPool.MatFrom("UI/Overlays/Designations/CutPlant", MatBases.MetaOverlay);
	}

	public Designation_CutPlant(Thing targetThing)
		: this()
	{
		target = new TargetPack(targetThing);
	}
}
