using System.Linq;

public static class HaulUtility
{
	public static Thing HaulableInSquare(IntVec3 loc)
	{
		return (from t in Find.Grids.ThingsAt(loc)
			where t.def.designateHaulable
			select t).FirstOrDefault();
	}
}
