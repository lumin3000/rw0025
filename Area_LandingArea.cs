public class Area_LandingArea : Building
{
	public IntVec3 HaulableDropSpot()
	{
		return GenMap.SpotToDropHaulableCloseTo(base.Position);
	}

	public IntVec3 RecruitSpawnSpot()
	{
		return base.Position;
	}
}
