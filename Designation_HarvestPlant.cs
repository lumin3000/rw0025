public class Designation_HarvestPlant : Designation
{
	public Designation_HarvestPlant()
	{
		dType = DesignationType.HarvestPlant;
		iconMat = MaterialPool.MatFrom("UI/Overlays/Designations/HarvestPlant", MatBases.MetaOverlay);
	}

	public Designation_HarvestPlant(Thing targetThing)
		: this()
	{
		target = new TargetPack(targetThing);
	}
}
