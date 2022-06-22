public class Designation_Mine : Designation
{
	public Designation_Mine()
	{
		dType = DesignationType.Mine;
		iconMat = MaterialPool.MatFrom("UI/Overlays/Designations/Mine", MatBases.MetaOverlay);
	}

	public Designation_Mine(IntVec3 targetLoc)
		: this()
	{
		target = new TargetPack(targetLoc);
	}
}
